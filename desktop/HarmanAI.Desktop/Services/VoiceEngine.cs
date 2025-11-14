using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace HarmanAI.Desktop.Services;

public class VoiceEngine
{
    private readonly SpeechRecognitionEngine _recognizer;
    private readonly SpeechSynthesizer _synthesizer;

    public event EventHandler<string>? CommandRecognized;

    public VoiceEngine()
    {
        _recognizer = new SpeechRecognitionEngine();
        _recognizer.LoadGrammar(new DictationGrammar());
        _recognizer.SpeechRecognized += (_, e) => CommandRecognized?.Invoke(this, e.Result.Text);
        _synthesizer = new SpeechSynthesizer();
    }

    public void Start() => _recognizer.RecognizeAsync(RecognizeMode.Multiple);
    public void Stop() => _recognizer.RecognizeAsyncStop();
    public void Speak(string text) => _synthesizer.SpeakAsync(text);
}
