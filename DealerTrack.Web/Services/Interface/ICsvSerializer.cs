using System.Collections.Generic;
using System.IO;

namespace DealerTrack.Web.Services.Interface
{
    public interface ICsvSerializer<T>
    {

         bool IgnoreEmptyLines { get; set; }
         bool IgnoreReferenceTypesExceptString { get; set; }
         string NewlineReplacement { get; set; }
         string Replacement { get; set; }
         string RowNumberColumnTitle { get; set; }
         char Separator { get; set; }
         string SplitRex { get; set; }
         bool UseEofLiteral { get; set; }
         bool UseLineNumbers { get; set; }
         bool UseTextQualifier { get; set; }

        IList<T> Deserialize(Stream stream);
        void Serialize(Stream stream, IList<T> data);
    }
}
