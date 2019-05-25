using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IQF.BizCommon.Helper
{
	public class PacketFactory
	{
		private static readonly Dictionary<int, Type> factory = new Dictionary<int, Type>();

		public static Type GetBodyType(int command)
		{
			return factory[command];
		}

		public static byte[] Pack(object body)
		{
			int cb = Marshal.SizeOf(body);
			byte[] destination = new byte[cb];
			IntPtr ptr = Marshal.AllocHGlobal(cb);
			Marshal.StructureToPtr(body, ptr, false);
			Marshal.Copy(ptr, destination, 0, destination.Length);
			Marshal.FreeHGlobal(ptr);
			return destination;
		}

		public static int GetPackSize(object body)
		{
			int cb = Marshal.SizeOf(body);
			return cb;
		}

		public static object ReadBody(byte[] data, Type bodyType)
		{
			int cb = Marshal.SizeOf(bodyType);
			if (cb > data.Length)
			{
				return null;
			}
			IntPtr destination = Marshal.AllocHGlobal(cb);
			Marshal.Copy(data, 0, destination, cb);
			object obj2 = Marshal.PtrToStructure(destination, bodyType);
			Marshal.FreeHGlobal(destination);
			return obj2;
		}
		public static T GetObject<T>(byte[] blob, int iStart)
		{
			int cb = Marshal.SizeOf(typeof(T));
			IntPtr destination = Marshal.AllocHGlobal(cb);
			try
			{
				Marshal.Copy(blob, iStart, destination, cb);
				return (T)Marshal.PtrToStructure(destination, typeof(T));
			}
			catch (OutOfMemoryException)
			{
				return default(T);
			}
			finally
			{
				Marshal.FreeHGlobal(destination);
			}
		}

		public static byte[] rawSerialize(object obj)
		{

			byte[] buffer2;

			int cb = Marshal.SizeOf(obj);

			IntPtr ptr = Marshal.AllocHGlobal(cb);

			try
			{
				Marshal.StructureToPtr(obj, ptr, false);
				byte[] destination = new byte[cb];
				Marshal.Copy(ptr, destination, 0, cb);
				buffer2 = destination;
			}

			finally
			{
				Marshal.FreeHGlobal(ptr);
			}
			return buffer2;
		}

		public static T rawDeserialize<T>(byte[] rawdatas)
		{

			Type anytype = typeof(T);

			int rawsize = Marshal.SizeOf(anytype);

			if (rawsize > rawdatas.Length) return default(T);

			IntPtr buffer = Marshal.AllocHGlobal(rawsize);

			Marshal.Copy(rawdatas, 0, buffer, rawsize);

			object retobj = Marshal.PtrToStructure(buffer, anytype);

			Marshal.FreeHGlobal(buffer);

			return (T)retobj;

		}

		public static List<T> GetStructArray<T>(byte[] bt)
		{
			Type anytype = typeof(T);

			int rawsize = Marshal.SizeOf(anytype);

			if (rawsize > bt.Length || bt.Length % rawsize != 0)
				return default(List<T>);

			int index = 0;
			List<T> tList = new List<T>();

			while (index != bt.Length)
			{
				byte[] btTemp = new byte[rawsize];
				Array.Copy(bt, index, btTemp, 0, rawsize);
				tList.Add(rawDeserialize<T>(btTemp));
				index += rawsize;
			}
			return tList;

		}

		public static T CopyData<T>(byte[] str, int start, int copylen)
		{
			T local;
			byte[] btall = new byte[copylen];
			Array.Copy(str, start, btall, 0, copylen);
			local = rawDeserialize<T>(btall);
			return local;
		}

		public static T CopyStruct<T>(T tin)
		{
			T local;
			byte[] btall = rawSerialize(tin);
			local = GetObject<T>(btall, 0);
			return local;
		}

		public static char transfer10to36(int a)
		{
			char creturn;

			int b = 10;
			if (a < b)
			{
				creturn = a.ToString().ToCharArray()[0];
			}
			else
			{
				creturn = (char)('A' + a - 10);
			}

			return creturn;
		}

		public static bool tranfer(Int64 is1, ref char[] acSymbol)
		{
			bool bret = true;
			string strK;
			strK = string.Format("{0:000}", is1);

			if (strK.Length == 17)
			{
				acSymbol[1 - 1] = transfer10to36(int.Parse(strK.Substring(5, 2), 0));
				acSymbol[2 - 1] = transfer10to36(int.Parse(strK.Substring(7, 2), 0));
				acSymbol[3 - 1] = transfer10to36(int.Parse(strK.Substring(9, 2), 0));
				acSymbol[4 - 1] = transfer10to36(int.Parse(strK.Substring(11, 2), 0));
				acSymbol[5 - 1] = transfer10to36(int.Parse(strK.Substring(13, 2), 0));
				acSymbol[6 - 1] = transfer10to36(int.Parse(strK.Substring(15, 2), 0));
			}
			else
				bret = false;
			return bret;
		}
	}

}
