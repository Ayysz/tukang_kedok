using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

    public class SplashScreen : MonoBehaviour
	{
		public CanvasGroup logoCanvasGroup; // Assign CanvasGroup dari Image Logo
		public float fadeDuration = 2f; // Durasi fade
		public float waitTime = 1.5f; // Waktu diam setelah fade in
		public float startWaitTime = 1f;
		public string nextScene = "MainMenu"; // Nama scene selanjutnya

		void Start()
		{
			StartCoroutine(PlaySplashScreen());
		}

		IEnumerator PlaySplashScreen()
		{
			yield return new WaitForSeconds(startWaitTime);
			logoCanvasGroup.alpha = 0; // Mulai dari transparan
			logoCanvasGroup.DOFade(1, fadeDuration); // Fade In
			yield return new WaitForSeconds(fadeDuration + waitTime);

			logoCanvasGroup.DOFade(0, fadeDuration); // Fade Out
			yield return new WaitForSeconds(fadeDuration);
			StartCoroutine(LoadYourAsyncScene());
			//SceneManager.LoadSceneAsync(nextScene); // Pindah ke Scene berikutnya
		}
        IEnumerator LoadYourAsyncScene()
        {
            // The Application loads the Scene in the background as the current Scene runs.
            // This is particularly good for creating loading screens.
            // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
            // a sceneBuildIndex of 1 as shown in Build Settings.

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    
}
