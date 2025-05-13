using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaCore.Domain.Clipping
{
    public class CabinetSaveClip
    {
        public int FileCabinetSavedClipId { get; set; }
        public int? ImageId { get; set; }
        public int? FileCabinetFolderId { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ClipName { get; set; }
        public string Coordinates { get; set; }
        public string ClipImagePath { get; set; }
        public int TagId { get; set; }
        public string Name { get; set; }
        public string FolderName { get; set; }
        public string Tag { get; set; }
        public DateTime? PubDate { get; set; }
        public string PubTitle { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string OCRText { get; set; }
        public string CountryName { get; set; }
        public string StateAbbr { get; set; }
        public string PubDateYear { get; set; }
        public DateTime DateAdded { get; set; }
        public int PageNum { get; set; }
        public int CountryId { get; set; }
        public string ExtractedNames { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int PubId { get; set; }
        public DateTimeOffset ClipDateAdded { get; set; }
        public int? ImageApproval { get; set; }
        public int pageNumber { get; set; }
        public string DisplayTime { get; set; } //added by devender singh/ks
        public bool isPublicClip { get; set; }
        public bool IsDeleted { get; set; }
        public int? OCRStatus { get; set; }
        public int? PngConvertedStatus { get; set; }
        public int ClipId { get; set; }
        public string TagName { get; set; }
        public bool? IsTagActive { get; set; }
        public int personId { get; set; }
        public string PType { get; set; }
        public int ClipTypeId { get; set; }
        public string personIds { get; set; }
        public int IsBlock { get; set; }
        public string BlockRequestBy { get; set; }
        public int PersonTaggedClipId { get; set; }
        public double angle { get; set; }
        public string ModUrl { get; set; }
        public string ObitText { get; set; }
        public int CDNClipId { get; set; }
        public bool IsPurged { get; set; }
        public string ProfileImagePath { get; set; }
        public string WebsiteUrl { get; set; }
        public string CountryAbbr { get; set; }
        public string pubtitleurl { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public string PubDateString { get; set; }
        public string DestImagePath { get; set; }
    }
}
