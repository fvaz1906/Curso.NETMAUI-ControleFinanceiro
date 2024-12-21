using Microsoft.Maui.Platform;

namespace oovaz_financeiro.Libraries.Utils.FixBugs
{
    public class KeyboardFixBugs
    {
        public static void hideKeyboard()
        {
#if ANDROID
            if (Platform.CurrentActivity.CurrentFocus != null)
            {
                Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
            }
#endif
        }
    }
}
