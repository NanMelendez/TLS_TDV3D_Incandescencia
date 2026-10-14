/*
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CodePuzzleManager : MonoBehaviour
{
    public const int CodeLength = 4;

    static readonly string[] Ordinals = { "primera", "segunda", "tercera", "cuarta" };
    static readonly string[] Prefixes = { "La ", "y la ", "La ", "la " };
    static readonly string[] Endings = { ",", ".", ",", "." };

    [Header("Pistas")]
    [SerializeField] List<ClueObject> clues = new();
    [Tooltip("Opcional: posiciones posibles. Las pistas elegidas se reparten al azar entre ellas.")]
    [SerializeField] List<Transform> clueSpawnPoints = new();
    [SerializeField] bool hideUnusedClues = true;
    [SerializeField] bool allowRepeatedDigits = false;

    [Header("Nota")]
    [SerializeField] TMP_Text poemText;

    [Header("Eventos")]
    public UnityEvent AbrirPuerta;

    int[] digits;
    int[] poemOrder;
    int[] expected;
    readonly Dictionary<int, ClueObject> clueByDigit = new();

    public string PoemText { get; private set; }

    void Awake()
    {
        GenerateCode();
        PlaceClues();
        BuildPoem();
        if (poemText != null) poemText.text = PoemText;
    }

    void GenerateCode()
    {
        if (allowRepeatedDigits)
            digits = Enumerable.Range(0, CodeLength).Select(_ => Random.Range(0, 10)).ToArray();
        else
            digits = Enumerable.Range(0, 10).OrderBy(_ => Random.value).Take(CodeLength).ToArray();

        poemOrder = Enumerable.Range(1, CodeLength).OrderBy(_ => Random.value).ToArray();
        expected = poemOrder.Select(p => digits[p - 1]).ToArray();

        Debug.Log($"[CodePuzzle] Dígitos (pos 1-4): {string.Join("", digits)} | Introducir: {string.Join("", expected)}");
    }

    void PlaceClues()
    {
        foreach (var c in clues) clueByDigit[c.digitValue] = c;

        foreach (int d in digits.Distinct())
            if (!clueByDigit.ContainsKey(d))
                Debug.LogError($"[CodePuzzle] Falta un ClueObject para el dígito {d}.");

        if (hideUnusedClues)
            foreach (var c in clues) c.gameObject.SetActive(digits.Contains(c.digitValue));

        var points = clueSpawnPoints.OrderBy(_ => Random.value).ToList();
        int i = 0;
        foreach (int d in digits.Distinct())
        {
            if (i >= points.Count) break;
            if (clueByDigit.TryGetValue(d, out var clue))
                clue.transform.SetPositionAndRotation(points[i++].position, points[i - 1].rotation);
        }
    }

    void BuildPoem()
    {
        string Line(int slot)
        {
            int pos = poemOrder[slot];
            string verse = clueByDigit.TryGetValue(digits[pos - 1], out var c) ? c.verse : "...";
            verse = verse.Trim().TrimEnd(',', '.', ';');
            return Prefixes[slot] + Ordinals[pos - 1] + " " + verse + Endings[slot];
        }

        PoemText =
            "Cuatro cifras dispersadas en la oscuridad,\n" +
            "ocultas en versos, sin alguna secuencialidad.\n" +
            Line(0) + "\n" +
            Line(1) + "\n\n" +
            Line(2) + "\n" +
            Line(3) + "\n" +
            "Si unes sus nombres siguiendo el cantar,\n" +
            "la puerta del misterio podrás desbloquear.";
    }

    public bool CheckCode(IReadOnlyList<int> input)
    {
        if (input.Count != CodeLength) return false;
        for (int i = 0; i < CodeLength; i++)
            if (input[i] != expected[i]) return false;
        return true;
    }

    public void Solve() => AbrirPuerta?.Invoke();
}
*/

using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CodePuzzleManager : MonoBehaviour
{
    public const int CodeLength = 4;

    static readonly string[] Ordinals = { "primera", "segunda", "tercera", "cuarta" };
    static readonly string[] Prefixes = { "La ", "y la ", "La ", "la " };
    static readonly string[] Endings = { ",", ".", ",", "." };

    [Header("Pistas")]
    [Tooltip("Prefabs de las pistas (del Project). Uno por dígito, cada uno con su ClueObject.")]
    [SerializeField] List<ClueObject> cluePrefabs = new();

    [Header("Puntos de aparición (una sola lista para todas las pistas)")]
    [Tooltip("Opcional: objeto vacío cuyos HIJOS son puntos de aparición. Se añaden solos.")]
    [SerializeField] Transform spawnPointsRoot;
    [Tooltip("Puntos sueltos adicionales. Pueden ser tantos como quieras (mínimo 4, o 10 con dígitos repetidos).")]
    [SerializeField] List<Transform> clueSpawnPoints = new();
    [Tooltip("Opcional: padre de las pistas instanciadas.")]
    [SerializeField] Transform cluesParent;
    [SerializeField] bool allowRepeatedDigits = false;

    [Header("Nota")]
    [SerializeField] TMP_Text poemText;

    [Header("Eventos")]
    public UnityEvent AbrirPuerta;

    int[] digits;
    int[] poemOrder;
    int[] expected;
    readonly Dictionary<int, ClueObject> prefabByDigit = new();

    public string PoemText { get; private set; }

    void Awake()
    {
        GenerateCode();
        SpawnClues();
        BuildPoem();
        if (poemText != null) poemText.text = PoemText;
    }

    void GenerateCode()
    {
        if (allowRepeatedDigits)
            digits = Enumerable.Range(0, CodeLength).Select(_ => Random.Range(0, 10)).ToArray();
        else
            digits = Enumerable.Range(0, 10).OrderBy(_ => Random.value).Take(CodeLength).ToArray();

        poemOrder = Enumerable.Range(1, CodeLength).OrderBy(_ => Random.value).ToArray();
        expected = poemOrder.Select(p => digits[p - 1]).ToArray();

        Debug.Log($"[CodePuzzle] Dígitos (pos 1-4): {string.Join("", digits)} | Introducir: {string.Join("", expected)}");
    }

    void SpawnClues()
    {
        prefabByDigit.Clear();
        foreach (var p in cluePrefabs)
        {
            if (p == null) continue;
            if (prefabByDigit.ContainsKey(p.digitValue))
                Debug.LogWarning($"[CodePuzzle] Hay dos prefabs con el dígito {p.digitValue}: '{p.name}' reemplaza a '{prefabByDigit[p.digitValue].name}'.", p);
            prefabByDigit[p.digitValue] = p;
        }

        var allPoints = new List<Transform>();
        if (spawnPointsRoot != null)
            foreach (Transform child in spawnPointsRoot) allPoints.Add(child);
        allPoints.AddRange(clueSpawnPoints.Where(p => p != null));

        var points = allPoints.Distinct().OrderBy(_ => Random.value).ToList();
        var usedDigits = digits.Distinct().ToList();

        if (points.Count < usedDigits.Count)
            Debug.LogWarning($"[CodePuzzle] Hay {points.Count} puntos para {usedDigits.Count} pistas; algunas no se instanciarán.", this);

        for (int i = 0; i < usedDigits.Count; i++)
        {
            int d = usedDigits[i];

            if (!prefabByDigit.TryGetValue(d, out var prefab))
            {
                Debug.LogError($"[CodePuzzle] Falta un prefab de pista para el dígito {d}.", this);
                continue;
            }
            if (i >= points.Count) continue;

            Instantiate(prefab, points[i].position, points[i].rotation, cluesParent);
            Debug.Log($"[CodePuzzle] Pista {d} ('{prefab.name}') -> '{points[i].name}'");
        }
    }

    void BuildPoem()
    {
        string Line(int slot)
        {
            int pos = poemOrder[slot];
            string verse = prefabByDigit.TryGetValue(digits[pos - 1], out var c) ? c.verse : "...";
            verse = verse.Trim().TrimEnd(',', '.', ';');
            return Prefixes[slot] + Ordinals[pos - 1] + " " + verse + Endings[slot];
        }

        PoemText =
            "Cuatro cifras dispersadas en la oscuridad,\n" +
            "ocultas en versos, sin alguna secuencialidad.\n" +
            Line(0) + "\n" +
            Line(1) + "\n\n" +
            Line(2) + "\n" +
            Line(3) + "\n" +
            "Si unes sus nombres siguiendo el cantar,\n" +
            "la puerta del misterio podrás desbloquear.";
    }

    public bool CheckCode(IReadOnlyList<int> input)
    {
        if (input.Count != CodeLength) return false;
        for (int i = 0; i < CodeLength; i++)
            if (input[i] != expected[i]) return false;
        return true;
    }

    public void Solve() => AbrirPuerta?.Invoke();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        var pts = new List<Transform>(clueSpawnPoints);
        if (spawnPointsRoot != null)
            foreach (Transform c in spawnPointsRoot) pts.Add(c);

        foreach (var p in pts)
        {
            if (p == null) continue;
            Gizmos.DrawWireSphere(p.position, 0.15f);
            Gizmos.DrawRay(p.position, p.forward * 0.4f);
        }
    }
}