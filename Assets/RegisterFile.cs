using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;

public class RegisterFile : MonoBehaviour
{
    public string filetype = ".bian";
    public string iconName = "MyIcon";
    string executePath;
    private void Awake()
    {
        executePath = System.Environment.GetCommandLineArgs()[0];
    }
    private void Start()
    {
        if (FileTypeRegister.FileTypeRegistered(filetype))
        {
            Debug.Log("文件类型已经存在~-"+filetype);
            FileTypeRegInfo regInfo =FileTypeRegister.GetFileTypeRegInfo(filetype);

            if (!File.Exists(regInfo.ExePath)||regInfo.ExePath!=executePath)
            {
                DialogResult dialogResult= MessageBox.Show(string.Format("文件系统{0}丢失打开方式,是否修复？", filetype),"警告:文件系统异常",MessageBoxButtons.OKCancel);
                if (dialogResult==DialogResult.OK)
                {
                    regInfo.ExePath = executePath;
                    regInfo.IconPath = Path.Combine(UnityEngine.Application.streamingAssetsPath, string.Format("{0}.ico", iconName));
                    FileTypeRegister.UpdateFileTypeRegInfo(regInfo);
                }
            }
        }
        else
        {
            Debug.LogWarning("文件类型不存在~-" + filetype);
            MessageBox.Show("文件系统尚未创建！");
        }
    }

    public void CreatRegData()
    {
        if (!FileTypeRegister.FileTypeRegistered(filetype))
        {
            Debug.LogFormat("文件类型 {0} 注册中！",filetype);
            FileTypeRegInfo fileTypeRegInfo = new FileTypeRegInfo(filetype)
            {
                Description = "测试自定义文件系统",
                ExePath = System.Environment.GetCommandLineArgs()[0],
                ExtendName = filetype,
                IconPath = Path.Combine(UnityEngine.Application.streamingAssetsPath, string.Format("{0}.ico", iconName))
            };

            // 注册
            FileTypeRegister fileTypeRegister = new FileTypeRegister();
            FileTypeRegister.RegisterFileType(fileTypeRegInfo);
        }
        else
        {
            Debug.LogFormat("文件类型 {0} 已经存在中！", filetype);
        }

    }

    public void UnRigisterData()
    {
        FileTypeRegister.UnRegisterFileType(filetype);
    }

}