using NaCore.Domain.Clipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaServices.Clipping
{
    public interface IClipping
    {
        List<ClippingTags> GetAllTags();
        List<ClippingTags> GetAllCategory();
        int SaveClipDetails(CabinetSaveClip clipData);
        CabinetSaveClip GetClipDetailsFromClipName(string clipName);
    }
}
