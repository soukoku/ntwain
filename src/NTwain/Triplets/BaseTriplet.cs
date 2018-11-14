namespace NTwain.Triplets
{
    public abstract class BaseTriplet
    {
        protected TwainSession session;

        public BaseTriplet(TwainSession session)
        {
            this.session = session;
        }
    }
}