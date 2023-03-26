using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using MapCoreLib.Core;
using MapCoreLib.Core.Scripts;
using MapCoreLib.Core.Util;
using WbInject;
using wbInject.Core;
using WbInjector.Core;

namespace wbInject
{
    public class WbStarter
    {
        private bool Running = false;

        private BlockingCollection<IpcData> messageQueue = new BlockingCollection<IpcData>();
        private Core.WbInjector wbInjector = new Core.WbInjector();

        public ICommandListener commandListener { get; set; }

        //TODO 这个还需要测试
        public async void initWbInjector()
        {
            if (Running)
            {
                //防止重复注入
                return;
            }
            
            initPipe();
            
            await Task.Delay(500);

            try
            { 
                wbInjector.injectDll2Wb();
                Running = true;
            }
            catch (Exception e)
            {
                Running = false;
                closeInjector();
                commandListener.showErrorBox(e.Message);
            }

        }

        public void refreshMap(string mapName)
        {
            // ScriptHandler.runScript(mapName);
            messageQueue.Add(new IpcData()
            {
                command = "refreshMap",
                param = new List<string>()
            });
        }

        public void closeInjector()
        {
            Logger.log($"closeInjector: Running {Running}");
            if (Running)
            {
                Logger.log("closeInjector");
                Running = false;
                messageQueue.Add(new IpcData()
                {
                    command = "quit"
                });
                messageQueue.CompleteAdding();
                wbInjector.detachDll();
            }
        }

        private void initPipe()
        {
            new Thread(PipeWriteProc).Start();
            new Thread(PipeReadProc).Start();
        }

        private void PipeReadProc(object data)
        {
            using (var readPipe = new NamedPipeServerStream("wbPipe2", PipeDirection.InOut))
            {
                using (var readStream = new PipeReaderWriter(readPipe))
                {
                    Logger.log("write pipe waiting");
                    readPipe.WaitForConnection();
                    Logger.log("write pipe connect");

                    try
                    {
                        //TODO pipe关闭监听
                        while (true)
                        {
                            IpcData ipcData = readStream.ReceiveCommand();

                            if (ipcData == null)
                            {
                                continue;
                            }

                            Logger.log($"readCommand: {ipcData}");
                            if (ipcData.command.Equals("quit"))
                            {
                                closeInjector();
                                break;
                            }
                            else
                            {
                                onWbCommand(ipcData);
                            }
                        }
                    }
                    catch (InvalidOperationException e)
                    {
                    }
                    catch (Exception e)
                    {
                        Logger.log(e.Message + "  " + e.StackTrace);
                        // readPipe.Close();
                        closeInjector();
                        Running = false;
                    }
                }
                
                
            }
        }

        private void onWbCommand(IpcData ipcData)
        {
            //TODO 检查地图文件存在
            switch (ipcData.command)
            {
                case "exeScript" :
                    exeScript(ipcData.param[0]);
                    break;
                case "exportXmlScript" :
                    exportXmlScript(ipcData.param[0]);
                    break;
                case "importXmlScript" :
                    importXmlScript(ipcData.param[0]);
                    break;
            }
        }

        private void exeScript(string mapName)
        {
            if (!CheckMapExist(mapName))
            {
                Logger.log("exeScript", $"map {mapName} not found");
                return;
            }
            var scriptName = commandListener.onSelectScript();
            Logger.log("exeScript", scriptName);
            if (scriptName == "" || scriptName == null)
            {
                return;
            }

            try
            { 
                ScriptHandler.runScript(mapName, scriptName);
            }
            catch (Exception e)
            {
                Logger.log(e.Message);
                commandListener.showErrorBox(e.Message);
            }
            
            refreshMap(mapName);
        }

        private bool CheckMapExist(string mapName)
        {
            return File.Exists(PathUtil.defaultMapPath(mapName));
        }

        private void importXmlScript(string mapName)
        {
            if (!CheckMapExist(mapName))
            {
                Logger.log("exeScript", $"map {mapName} not found");
                return;
            }
            try
            { 
                ScriptXml.deserialize(PathUtil.defaultMapPath(mapName));
                refreshMap(mapName);
            }
            catch (Exception e)
            {
                Logger.log("importXmlScript", $"{e.Message} \n {e.StackTrace}");
                commandListener.showErrorBox(e.Message);
            }
            
        }

        private void exportXmlScript(string mapName)
        {
            if (!CheckMapExist(mapName))
            {
                Logger.log("exeScript", $"map {mapName} not found");
                return;
            }
            try
            { 
                ScriptXml.serialize(PathUtil.defaultMapPath(mapName));
                EditorHelper.openEditor4XmlScript(mapName);
            }
            catch (Exception e)
            {
                Logger.log("importXmlScript", $"{e.Message} \n {e.StackTrace}");
                commandListener.showErrorBox(e.Message);
            }
        }

        private void PipeWriteProc(object data)
        {
            using (var pipeServer = new NamedPipeServerStream("wbPipe1", PipeDirection.InOut))
            {
                using (var writeStream = new PipeReaderWriter(pipeServer))
                {
                    Logger.log("read pipe waiting");
                    pipeServer.WaitForConnection();
                    Logger.log("read pipe connect");
                    writeStream.SendCommand(new IpcData()
                    {
                        command = "init",
                        param = new List<string>()
                        {
                            wbInjector.wbHWND.ToString()
                        }
                    });

                    try
                    {
                        //TODO pipe关闭监听
                        while (true)
                        {
                            var ipcData = messageQueue.Take();
                            Logger.log($"write pipe msg: {ipcData.command}");
                            writeStream.SendCommand(ipcData);
                            pipeServer.WaitForPipeDrain();
                            if (ipcData.command.Equals("quit"))
                            {
                                break;
                            }
                        }
                    }
                    catch (InvalidOperationException e)
                    {
                    }
                    catch (Exception e)
                    {
                        Logger.log(e.Message + "  " + e.StackTrace);
                        pipeServer.Close();
                    }
                }
                
            }
            
            Thread.Sleep(800);
            commandListener.onClose();
        }

        public void detachDll()
        {
            wbInjector.detachDll();
        }

        public interface ICommandListener
        {
            /**
             * 弹出对话框选择代码文件，返回文件路径并且执行修改地图
             */
            string onSelectScript();

            void onClose();

            void showErrorBox(string msg);
        }
    }
}