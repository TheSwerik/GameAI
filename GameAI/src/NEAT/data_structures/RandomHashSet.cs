public class RandomHashSet<T>
{
    public RandomHashSet()
    {
        set = new HashSet<>();
        data = new ArrayList<>();
    }

    private final HashSet<T>set;
    private final ArrayList<T>data;

    public boolean contains(T object)

    public T random_element() { return set.size() > 0 ? data.get(Math.random() * size()) : null; }

    public int size() { return data.size(); }

    public void add(T object)

    public void clear()
    {
        set.clear();
        data.clear();
    }

    public T get(int index) { return index >= 0 && index < size() ? data.get(index) : null; }

    public void remove(int index)
    {
        if (index < 0 || index >= size()) return;
        set.remove(data.get(index));
        data.remove(index);
    }

    public void remove(T object)

    public ArrayList<T> getData() { return data; }
}