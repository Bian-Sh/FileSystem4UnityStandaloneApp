using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
/// <summary>  
/// 注册自定义的文件类型。  
/// </summary>  
public class FileTypeRegister
{
    [DllImport("shell32.dll")]
    public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
    /// <summary>  
    /// 使文件类型与对应的图标及应用程序关联起来
    /// </summary>          
    public static void RegisterFileType(FileTypeRegInfo regInfo)
    {
        if (FileTypeRegistered(regInfo.ExtendName))
        {

            return;
        }
        try
        {
            UnityEngine.Debug.Log("注册表添加文件系统中！");

            //HKEY_CLASSES_ROOT/.osf
            RegistryKey fileTypeKey = Registry.ClassesRoot.CreateSubKey(regInfo.ExtendName);
            string relationName = regInfo.ExtendName.Substring(1, regInfo.ExtendName.Length - 1).ToUpper() + "_FileType";
            fileTypeKey.SetValue("", relationName);
            fileTypeKey.Close();

            //HKEY_CLASSES_ROOT/OSF_FileType
            RegistryKey relationKey = Registry.ClassesRoot.CreateSubKey(relationName);
            relationKey.SetValue("", regInfo.Description);

            //HKEY_CLASSES_ROOT/OSF_FileType/Shell/DefaultIcon
            RegistryKey iconKey = relationKey.CreateSubKey("DefaultIcon");
            iconKey.SetValue("", regInfo.IconPath);

            //HKEY_CLASSES_ROOT/OSF_FileType/Shell
            RegistryKey shellKey = relationKey.CreateSubKey("Shell");

            //HKEY_CLASSES_ROOT/OSF_FileType/Shell/Open
            RegistryKey openKey = shellKey.CreateSubKey("Open");

            //HKEY_CLASSES_ROOT/OSF_FileType/Shell/Open/Command
            RegistryKey commandKey = openKey.CreateSubKey("Command");
            commandKey.SetValue("", regInfo.ExePath + " %1"); // " %1"表示将被双击的文件的路径传给目标应用程序
            relationKey.Close();
            SHChangeNotify(0x8000000, 0, IntPtr.Zero, IntPtr.Zero);

        }
        catch (Exception)
        {
            UnityEngine.Debug.Log("注册表添加文件系统失败！");
            DialogResult dialogResult = MessageBox.Show("重启并尝试以管理员身份运行？", "友情提示-" + UnityEngine.Application.productName, MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                ReStartAs();
            }
            throw;
        }


    }


    public static bool UnRegisterFileType(string extendName)
    {
        if (FileTypeRegistered(extendName))
        {
            try
            {
                string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";
                using (RegistryKey rootReg = Registry.ClassesRoot)
                {
                    rootReg.DeleteSubKey(extendName);
                    //rootReg.OpenSubKey(relationName);
                    rootReg.DeleteSubKeyTree(relationName);
                }
                SHChangeNotify(0x8000000, 0, IntPtr.Zero, IntPtr.Zero);
                return true;
            }
            catch (System.Exception)
            {
                UnityEngine.Debug.LogError("删除注册表操作失败！");
                DialogResult dialogResult = MessageBox.Show("重启并尝试以管理员身份运行？", "友情提示-" + UnityEngine.Application.productName, MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    ReStartAs();
                }
                return false;
            }

        }
        else
        {
            UnityEngine.Debug.Log("系统不存在文件类型：" + extendName);
            return true;
        }
    }
    /// <summary>  
    /// 更新指定文件类型关联信息  
    /// </summary>      
    public static bool UpdateFileTypeRegInfo(FileTypeRegInfo regInfo)
    {
        if (!FileTypeRegistered(regInfo.ExtendName))
        {
            return false;
        }

        string extendName = regInfo.ExtendName;
        string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";
        RegistryKey relationKey = Registry.ClassesRoot.OpenSubKey(relationName, true);
        relationKey.SetValue("", regInfo.Description);
        RegistryKey iconKey = relationKey.OpenSubKey("DefaultIcon", true);
        iconKey.SetValue("", regInfo.IconPath);
        RegistryKey shellKey = relationKey.OpenSubKey("Shell");
        RegistryKey openKey = shellKey.OpenSubKey("Open");
        RegistryKey commandKey = openKey.OpenSubKey("Command", true);
        commandKey.SetValue("", regInfo.ExePath + " %1");
        relationKey.Close();
        return true;
    }

    /// <summary>  
    /// 获取指定文件类型关联信息  
    /// </summary>          
    public static FileTypeRegInfo GetFileTypeRegInfo(string extendName)
    {
        if (!FileTypeRegistered(extendName))
        {
            return null;
        }
        FileTypeRegInfo regInfo = new FileTypeRegInfo(extendName);

        string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";
        RegistryKey relationKey = Registry.ClassesRoot.OpenSubKey(relationName);
        regInfo.Description = relationKey.GetValue("").ToString();
        RegistryKey iconKey = relationKey.OpenSubKey("DefaultIcon");
        regInfo.IconPath = iconKey.GetValue("").ToString();
        RegistryKey shellKey = relationKey.OpenSubKey("Shell");
        RegistryKey openKey = shellKey.OpenSubKey("Open");
        RegistryKey commandKey = openKey.OpenSubKey("Command");
        string temp = commandKey.GetValue("").ToString();
        regInfo.ExePath = temp.Substring(0, temp.Length - 3);
        return regInfo;
    }

    /// <summary>  
    /// 指定文件类型是否已经注册  
    /// </summary>          
    public static bool FileTypeRegistered(string extendName)
    {
        RegistryKey softwareKey = Registry.ClassesRoot.OpenSubKey(extendName);
        if (softwareKey != null)
        {
            UnityEngine.Debug.Log(softwareKey.Name);

            return true;
        }
        UnityEngine.Debug.Log("12312");
        return false;
    }
    private static void ReStartAs()
    {
        //创建启动对象 
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
        {
            //设置运行文件 
            FileName = System.Environment.GetCommandLineArgs()[0],
            //设置启动参数 
            Arguments = "",
            //设置启动动作,确保以管理员身份运行 
            Verb = "runas"
        };
        //如果不是管理员，则启动UAC 
        System.Diagnostics.Process.Start(startInfo);
        //退出 
        UnityEngine.Application.Quit();
    }


}
