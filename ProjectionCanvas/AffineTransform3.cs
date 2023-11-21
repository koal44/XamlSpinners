namespace ProjectionCanvas
{
    public abstract class AffineTransform3 : Transform3
    {
        public AffineTransform3() { }

        public override bool IsAffine
        {
            get { ReadPreamble(); return true; }
        }

        public new AffineTransform3 Clone() => (AffineTransform3)base.Clone();

        public new AffineTransform3 CloneCurrentValue() => (AffineTransform3)base.CloneCurrentValue();

    }
}