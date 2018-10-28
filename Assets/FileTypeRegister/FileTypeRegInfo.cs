/// <summary>
/// 文件类型注册信息
/// </summary>
public class FileTypeRegInfo
{
    /// <summary>  
    /// 扩展名  
    /// </summary>  
    public string ExtendName;  //".osf"  
    /// <summary>  
    /// 说明  
    /// </summary>  
    public string Description; //"OpenSelfFile项目文件"  
    /// <summary>  
    /// 关联的图标  
    /// </summary>  
    public string IconPath;
    /// <summary>  
    /// 应用程序  
    /// </summary>  
    public string ExePath;

    public FileTypeRegInfo()
    {
    }
    public FileTypeRegInfo(string extendName)
    {
        this.ExtendName = extendName;
    }
}
