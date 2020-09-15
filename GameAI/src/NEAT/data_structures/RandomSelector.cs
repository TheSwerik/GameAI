public class RandomSelector<T>
{
    private double total_score;

    private final ArrayList<T>objects=
    ArrayList<>();
    private final ArrayList<Double>scores=
    ArrayList<>();

    public void add(T element, double score)
    {
        objects.add(element);
        scores.add(score);
        total_score += score;
    }

    public T random()
    {
        var v = Math.random() * total_score;

        double c = 0;
        for (var i = 0; i < objects.size(); i++)
        {
            c += scores.get(i);
            if (c > v) return objects.get(i);
        }

        return null;
    }

    public void reset()
    {
        objects.clear();
        scores.clear();
        total_score = 0;
    }
}