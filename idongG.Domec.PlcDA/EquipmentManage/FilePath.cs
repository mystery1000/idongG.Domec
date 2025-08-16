using System;

namespace idongG.Domec.PlcDA.EquipmentManage;

  public static class FilePath
  {
      public static DirectoryInfo ModelPath;

      /// <summary>
      /// 数据库路径
      /// </summary>
      public static DirectoryInfo DatabasePath;

      /// <summary>
      /// 图像路径
      /// </summary>
      public static DirectoryInfo ImagePath;

      /// <summary>
      /// 图像路径
      /// </summary>
      public static DirectoryInfo NGImagePath;

      /// <summary>
      /// 日志路径
      /// </summary>
      public static DirectoryInfo LogPath;

      /// <summary>
      /// 产品路径
      /// </summary>
      public static DirectoryInfo ProductPath;

      /// <summary>
      /// 生产结果数据
      /// </summary>
      public static DirectoryInfo ProductDataPath;

      /// <summary>
      /// 配方路径
      /// </summary>
      public static DirectoryInfo RecipePath;

      /// <summary>
      /// 产品文件具体路径
      /// </summary>
      public static FileInfo SetFilePath;

      /// <summary>
      /// 生产过程数据具体路径
      /// </summary>
      public static FileInfo ProductDetailPath;

      /// <summary>
      /// 程序用时文件具体路径
      /// </summary>
      public static FileInfo RunTimeFilePath;

      /// <summary>
      /// 程序用时路径
      /// </summary>
     // public static DirectoryInfo RunTimePath;

      /// <summary>
      /// 插件文件夹路径
      /// </summary>
      public static DirectoryInfo PluginPath;

      /// <summary>
      /// 设定路径
      /// </summary>
      public static DirectoryInfo SetPath;

      /// <summary>
      /// 生产图片文件夹
      /// </summary>
      public static DirectoryInfo ProductImagePath;

      /// <summary>
      /// AutoFocus文件夹
      /// </summary>
      public static DirectoryInfo AutoFocusImagePath;

      /// <summary>
      /// 日志路径
      /// </summary>
      public static DirectoryInfo FlowLogPath;

      public static DirectoryInfo ToolLogPath;
      public static DirectoryInfo ModuleLogPath;
      public static DirectoryInfo MasterLogPath;
      public static DirectoryInfo MachineLogPath;
      public static DirectoryInfo MovementLogPath;
      public static DirectoryInfo HardwareLogPath;
      public static DirectoryInfo WorkshopLogPath;
      public static DirectoryInfo TrayLogPath;
      public static DirectoryInfo WarehauseLogPath;
      public static DirectoryInfo BlindingsLogPath;
      public static DirectoryInfo VisionLogPath;

      /// <summary>
      /// 判断文件是否存在,没有并创建
      /// </summary>
      /// <returns></returns>
      public static bool CheckFileExit()
      {
          LogPath = new DirectoryInfo(string.Format("{0}\\Log", AppDomain.CurrentDomain.BaseDirectory));

          FlowLogPath = new DirectoryInfo($"{LogPath.FullName}\\FlowLog");
          ToolLogPath = new DirectoryInfo($"{LogPath.FullName}\\ToolLog");
          ModuleLogPath = new DirectoryInfo($"{LogPath.FullName}\\ModuleLog");
          MasterLogPath = new DirectoryInfo($"{LogPath.FullName}\\MasterLog");
          MachineLogPath = new DirectoryInfo($"{LogPath.FullName}\\MachineLog");
          MovementLogPath = new DirectoryInfo($"{LogPath.FullName}\\MovementLog");
          HardwareLogPath = new DirectoryInfo($"{LogPath.FullName}\\HardwareLog");
          WorkshopLogPath = new DirectoryInfo($"{LogPath.FullName}\\WorkshopLog");
          TrayLogPath = new DirectoryInfo($"{LogPath.FullName}\\TrayLog");
          WarehauseLogPath = new DirectoryInfo($"{LogPath.FullName}\\WarehauseLog");
          BlindingsLogPath = new DirectoryInfo($"{LogPath.FullName}\\BlindingsLog");
          VisionLogPath = new DirectoryInfo($"{LogPath.FullName}\\VisionLog");

          ProductPath = new DirectoryInfo(string.Format("{0}\\Product\\", AppDomain.CurrentDomain.BaseDirectory));
          SetPath = new DirectoryInfo(string.Format("{0}\\Set\\", AppDomain.CurrentDomain.BaseDirectory));
          ImagePath = new DirectoryInfo(string.Format("{0}", "D:\\Data\\ProductImage"));
          NGImagePath = new DirectoryInfo(string.Format("{0}", "D:\\Data\\NGImage"));
          DatabasePath = new DirectoryInfo(string.Format("{0}\\Database\\", AppDomain.CurrentDomain.BaseDirectory));
          RecipePath = new DirectoryInfo(string.Format("{0}\\Recipe\\", AppDomain.CurrentDomain.BaseDirectory));
          PluginPath = new DirectoryInfo(string.Format("{0}\\Plugin\\", AppDomain.CurrentDomain.BaseDirectory));
          ProductImagePath = new DirectoryInfo(string.Format("D:\\Data\\ProductImage"));
          ProductDataPath = new DirectoryInfo(string.Format("D:\\Data\\ProductData"));
          AutoFocusImagePath = new DirectoryInfo(string.Format("{0}\\AutoFocus\\", AppDomain.CurrentDomain.BaseDirectory));
          ModelPath = new DirectoryInfo(string.Format("{0}\\Model\\", AppDomain.CurrentDomain.BaseDirectory));

          // RunTimePath = new DirectoryInfo(string.Format("{0}\\RunTimePath\\", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86)));

          if (!LogPath.Exists) LogPath.Create();
          if (!ProductPath.Exists) ProductPath.Create();
          if (!SetPath.Exists) SetPath.Create();
          if (!ImagePath.Exists) ImagePath.Create();
          if (!NGImagePath.Exists) ImagePath.Create();
          if (!DatabasePath.Exists) DatabasePath.Create();
          if (!RecipePath.Exists) RecipePath.Create();
          //  if (!RunTimePath.Exists) RunTimePath.Create();
          if (!PluginPath.Exists) PluginPath.Create();
          if (!ProductImagePath.Exists) ProductImagePath.Create();
          if (!ProductDataPath.Exists) ProductDataPath.Create();
          if (!AutoFocusImagePath.Exists) AutoFocusImagePath.Create();
          if (!ModelPath.Exists) ModelPath.Create();

          if (!FlowLogPath.Exists) FlowLogPath.Create();
          if (!ToolLogPath.Exists) ToolLogPath.Create();
          if (!ModuleLogPath.Exists) ModuleLogPath.Create();
          if (!MasterLogPath.Exists) MasterLogPath.Create();
          if (!MachineLogPath.Exists) MachineLogPath.Create();
          if (!MovementLogPath.Exists) MovementLogPath.Create();
          if (!HardwareLogPath.Exists) HardwareLogPath.Create();
          if (!TrayLogPath.Exists) TrayLogPath.Create();
          if (!BlindingsLogPath.Exists) BlindingsLogPath.Create();
          if (!VisionLogPath.Exists) VisionLogPath.Create();

          SetFilePath = new FileInfo(string.Format("{0}Set.json", SetPath.FullName));
          ProductDetailPath = new FileInfo(string.Format("{0}ProductDetail.json", SetPath.FullName));
          return true;
      }

      /// <summary>
      /// 文件夹内的文件复制
      /// </summary>
      /// <param name="source"></param>
      /// <param name="target"></param>
      public static void CopyFolder(DirectoryInfo source, DirectoryInfo target)
      {
          if (!target.Exists) target.Create();

          foreach (FileInfo file in source.GetFiles())
          {
              file.CopyTo(Path.Combine(target.FullName, file.Name), true);
          }

          foreach (DirectoryInfo subDir in source.GetDirectories())
          {
              DirectoryInfo nextTarget = target.CreateSubdirectory(subDir.Name);
              CopyFolder(subDir, nextTarget);
          }
      }
  }
