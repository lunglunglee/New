using System.Activities;
using System.ComponentModel;

namespace BasicIdentity_SxSExecution
{
    public class WaitForInput<T> : NativeActivity<T>
    {
        [DefaultValue(null)]
        public string BookmarkName { get; set; }

        protected override bool CanInduceIdle
        {
            get { return true; }
        }

        protected override void Execute(NativeActivityContext context)
        {
            context.CreateBookmark(this.BookmarkName, Continue);
        }

        void Continue(NativeActivityContext context, Bookmark bookmark, object state)
        {
            Result.Set(context, state);
        }
    }
}
