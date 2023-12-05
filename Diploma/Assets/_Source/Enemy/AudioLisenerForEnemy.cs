using UnityEngine;

public class AudioLisenerForEnemy : MonoBehaviour
{
    void Start()
    {
        // Получаем доступ к микрофону
        string microphone = Microphone.devices[0]; // Первый доступный микрофон
        int bufferSize = 4096; // Размер буфера для считывания аудио данных (можно изменить)
        int frequency = 44100; // Частота дискретизации аудио данных (можно изменить)

        // Запускаем микрофон
        AudioClip clip = Microphone.Start(microphone, true, 1, frequency);
        while (Microphone.GetPosition(null) <= 0) { } // Ждем, пока микрофон не запустится

        // Читаем аудио данные из микрофона и рассчитываем громкость
        float[] samples = new float[bufferSize];
        float volume = 0f;
        while (true)
        {
            // Читаем аудио данные в буфер
            int position = Microphone.GetPosition(null) - bufferSize;
            if (position < 0) continue; // Если буфер еще не заполнен, продолжаем ожидать
            clip.GetData(samples, position); // Считываем аудио данные

            // Рассчитываем громкость
            for (int i = 0; i < bufferSize; i++)
            {
                volume += Mathf.Abs(samples[i]);
            }
            volume /= bufferSize;

            // Используем полученную громкость по своему усмотрению (например, выводим в консоль)
            Debug.Log("Microphone volume: " + volume);

            // При необходимости можно добавить задержку в цикле для снижения нагрузки на процессор
            // System.Threading.Thread.Sleep(16); // Задержка в 16 миллисекунд (60 кадров в секунду)
        }

        // Останавливаем микрофон
        Microphone.End(microphone);
    }
}
