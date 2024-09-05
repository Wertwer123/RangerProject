using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace RangerProject.Scripts.Utils
{
    [StructLayout( LayoutKind.Explicit ), Serializable]
    public class SerializableGUID : IComparable, IEquatable<SerializableGUID>
    {
        [FieldOffset(0)]
        public Guid Guid = Guid.NewGuid( );
        [FieldOffset(0), SerializeField]
        private Int32 GuidPart1;
        [FieldOffset(4), SerializeField]
        private Int32 GuidPart2;
        [FieldOffset(8), SerializeField]
        private Int32 GuidPart3;
        [FieldOffset(12), SerializeField]
        private Int32 GuidPart4;

        public static implicit operator Guid (SerializableGUID UGuid )
        {
            return UGuid.Guid;
        }

        public Int32 CompareTo ( object obj )
        {
            if( obj == null )
                return -1;

            if( obj is SerializableGUID )
                return ((SerializableGUID)obj).Guid.CompareTo( Guid );

            if( obj is Guid guid )
                return guid.CompareTo(Guid);

            return -1;
        }
        public Int32 CompareTo ( Guid other )
        {
            return Guid.CompareTo( other );
        }
        public Boolean Equals ( Guid other )
        {
            return Guid == other;
        }

        public bool Equals(SerializableGUID other)
        {
            return other != null && other.Guid == Guid;
        }

        public override Boolean Equals ( object obj )
        {
            if( obj == null )
                return false;

            if( obj is SerializableGUID )
                return (SerializableGUID)obj == Guid;

            if( obj is Guid other)
                return other == Guid;

            return false;
        }
        public override Int32 GetHashCode ( )
        {
            return Guid.GetHashCode( );
        }
    }
}
