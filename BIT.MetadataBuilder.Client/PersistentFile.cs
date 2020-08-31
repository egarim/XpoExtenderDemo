using DevExpress.Utils;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;


namespace BIT.XpoExtender.Client
{
	public class CompressionConverter : ValueConverter
	{
		public override object ConvertToStorageType(object value)
		{
			if (value != null && !(value is byte[]))
			{
				throw new ArgumentException();
			}
			if (value == null || ((byte[])value).Length == 0)
			{
				return value;
			}
			return CompressionUtils.Compress(new MemoryStream((byte[])value)).ToArray();
		}
		public override object ConvertFromStorageType(object value)
		{
			if (value != null && !(value is byte[]))
			{
				throw new ArgumentException();
			}
			if (value == null || ((byte[])value).Length == 0)
			{
				return value;
			}
			return CompressionUtils.Decompress(new MemoryStream((byte[])value)).ToArray();
		}
		public override Type StorageType
		{
			get { return typeof(byte[]); }
		}
	}
	[DefaultProperty(nameof(FileName))]
	public class PersistentFile : XpoExtenderBaseObject
	{
		private string fileName = "";
#if MediumTrust
		private int size;
		public int Size {
			get { return size; }
			set { SetPropertyValue("Size", ref size, value); }
		}
#else
		[Persistent]
		private int size;
		public int Size
		{
			get { return size; }
		}
#endif
		public PersistentFile(Session session) : base(session) { }
		public virtual void LoadFromStream(string fileName, Stream stream)
		{
			Guard.ArgumentNotNull(stream, "stream");
			FileName = fileName;
			byte[] bytes = new byte[stream.Length];
			stream.Read(bytes, 0, bytes.Length);
			Content = bytes;
		}
		public virtual void SaveToStream(Stream stream)
		{
			if (Content != null)
			{
				stream.Write(Content, 0, Size);
			}
			stream.Flush();
		}
		public void Clear()
		{
			Content = null;
			FileName = String.Empty;
		}
		public override string ToString()
		{
			return FileName;
		}
		[Size(260)]
		public string FileName
		{
			get { return fileName; }
			set { SetPropertyValue(nameof(FileName), ref fileName, value); }
		}
		[Persistent, Delayed(true)]
		[ValueConverter(typeof(CompressionConverter))]
		[MemberDesignTimeVisibility(false)]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public byte[] Content
		{
			get { return GetDelayedPropertyValue<byte[]>(nameof(Content)); }
			set
			{
				int oldSize = size;
				if (value != null)
				{
					size = value.Length;
				}
				else
				{
					size = 0;
				}
				SetDelayedPropertyValue(nameof(Content), value);
				OnChanged(nameof(Size), oldSize, size);
			}
		}
		#region IEmptyCheckable Members
		[NonPersistent, MemberDesignTimeVisibility(false)]
		public bool IsEmpty
		{
			get { return string.IsNullOrEmpty(FileName); }
		}
		#endregion
	}
}