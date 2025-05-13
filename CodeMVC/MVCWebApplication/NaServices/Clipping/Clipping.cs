using NaCore.Domain.Clipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaServices.Clipping
{
    public class Clipping : IClipping
    {
        public List<ClippingTags> GetAllCategory()
        {
            throw new NotImplementedException();
        }

        public List<ClippingTags> GetAllTags()
        {
            throw new NotImplementedException();
        }

        public CabinetSaveClip GetClipDetailsFromClipName(string clipName)
        {
            throw new NotImplementedException();
        }

        public int SaveClipDetails(CabinetSaveClip clipData)
        {
            throw new NotImplementedException();
        }
    }
}
