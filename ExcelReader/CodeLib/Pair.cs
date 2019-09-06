using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.CodeLib
{
    public class Pair<TLeft, TRight> : IEquatable<Pair<TLeft, TRight>>
    {
        public TLeft Left;
        public TRight Right;

        public Pair(TLeft left, TRight right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", Left, Right);
        }

        public override int GetHashCode()
        {
            return Left.GetHashCode() + Right.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Pair<TLeft, TRight>)
            {
                return this.Equals((Pair<TLeft, TRight>)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Pair<TLeft, TRight> other)
        {
            return this.Left.Equals(other.Left) && this.Right.Equals(other.Right);
        }

        public static bool operator ==(Pair<TLeft, TRight> one, Pair<TLeft, TRight> other)
        {
            return one.Equals(other);
        }

        public static bool operator !=(Pair<TLeft, TRight> one, Pair<TLeft, TRight> other)
        {
            return !(one == other);
        }
    }
}
