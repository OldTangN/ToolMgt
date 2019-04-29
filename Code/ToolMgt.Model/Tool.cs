//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToolMgt.Model
{
    using System;
    using System.Collections.Generic;
    using GalaSoft.MvvmLight;
    
    public partial class Tool: ObservableObject
    {
     
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tool()
        {
            this.Barcode = "";
            this.RFID = "";
            this.Name = "";
            this.Spec = "";
            this.Manufacturer = "";
            this.ManufacturingCode = "";
            this.Note = "";
            this.ToolRecords = new HashSet<ToolRecord>();
        }
    
        private int _Id;
        public int Id { get => _Id; set => Set(ref _Id, value); }
        private string _Barcode;
        public string Barcode { get => _Barcode; set => Set(ref _Barcode, value); }
        private string _RFID;
        public string RFID { get => _RFID; set => Set(ref _RFID, value); }
        private string _Name;
        public string Name { get => _Name; set => Set(ref _Name, value); }
        private int _CategoryId;
        public int CategoryId { get => _CategoryId; set => Set(ref _CategoryId, value); }
        private bool _IsMeasureTool;
        public bool IsMeasureTool { get => _IsMeasureTool; set => Set(ref _IsMeasureTool, value); }
        private bool _IsPkg;
        public bool IsPkg { get => _IsPkg; set => Set(ref _IsPkg, value); }
        private string _Spec;
        public string Spec { get => _Spec; set => Set(ref _Spec, value); }
        private string _Manufacturer;
        public string Manufacturer { get => _Manufacturer; set => Set(ref _Manufacturer, value); }
        private string _ManufacturingCode;
        public string ManufacturingCode { get => _ManufacturingCode; set => Set(ref _ManufacturingCode, value); }
        private System.DateTime _BuyDate;
        public System.DateTime BuyDate { get => _BuyDate; set => Set(ref _BuyDate, value); }
        private int _CheckCycle;
        public int CheckCycle { get => _CheckCycle; set => Set(ref _CheckCycle, value); }
        private Nullable<System.DateTime> _CheckNextDate;
        public Nullable<System.DateTime> CheckNextDate { get => _CheckNextDate; set => Set(ref _CheckNextDate, value); }
        private string _Note;
        public string Note { get => _Note; set => Set(ref _Note, value); }
        private int _StateId;
        public int StateId { get => _StateId; set => Set(ref _StateId, value); }
        private int _Qty;
        public int Qty { get => _Qty; set => Set(ref _Qty, value); }
    
        public virtual ToolState ToolState { get; set; }
        public virtual ToolCategory ToolCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ToolRecord> ToolRecords { get; set; }
    }
}