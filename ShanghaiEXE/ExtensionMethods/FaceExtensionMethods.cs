using Common;

namespace ExtensionMethods
{
    public static class FaceExtensionMethods
    {
        public static FACE ToFace(this FaceId fid)
        {
            if (fid.Sheet == 0)
            {
                return FACE.None;
            }

            return (FACE)(((fid.Sheet - 1) * 16) + fid.Index);
        }

        public static FaceId ToFaceId(this FACE face) => face.ToFaceId(false);

        public static FaceId ToFaceId(this FACE face, bool mono)
        {
            if (face == FACE.None)
            {
                return new FaceId(0, 0, mono);
            }

            var id = (int)face;

            return new FaceId((id / 16) + 1, (byte)(id % 16), mono);
        }
    }
}
