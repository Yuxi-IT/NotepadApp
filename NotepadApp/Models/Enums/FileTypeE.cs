using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotepadApp.Models.Enums
{
    public enum FileTypeE
    {
        // 源代码
        Python,
        CSharp,
        Java,
        C,
        CPP,
        CSS,
        HTML,
        TypeScript,
        JavaScript,
        Vue,
        PHP,
        Class,
        MarkdownV2,
        MarkdownV3,

        // 储存格式

        Json,
        Log,
        INI,
        Sln,
        Xml,
        Xaml,
        Config,
        CSV,
        YAML,
        YML,

        // 其他格式
        Text,
        GITIGNORE,
        HTTP,
        RDP,
    }
}
