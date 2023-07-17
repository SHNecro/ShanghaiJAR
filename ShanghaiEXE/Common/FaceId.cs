﻿using ExtensionMethods;
using System.Security.Cryptography;
using System;

namespace Common
{
    public class FaceId
    {
        public FaceId(int sheet, byte index, bool mono)
        {
            this.Sheet = sheet;
            this.Index = index;
            this.Mono = mono;
        }

        public FaceId() : this(0, 0, false) { }

        public int Sheet { get; }
        public byte Index { get; }

        public bool Mono { get; }

        public override string ToString()
		{
			var faceEnum = ((this.Sheet - 1) * 16) + this.Index;
			if (Enum.IsDefined(typeof(FACE), faceEnum))
			{
				return this.ToFace().ToString();
			}
            else
            {
                return $"{this.Sheet},{this.Index}";
            }
        }
    }
}
