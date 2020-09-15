package data_structures;

import java.util.ArrayList;
import java.util.HashSet;

public class RandomHashSet<T> {

    private final HashSet<T> set;
    private final ArrayList<T> data;

    public RandomHashSet() {
        set = new HashSet<>();
        data = new ArrayList<>();
    }

    public boolean contains(T object) {
        return set.contains(object);
    }

    public T random_element() {
        return set.size() > 0 ? data.get((int) (Math.random() * size())) : null;
    }

    public int size() {
        return data.size();
    }

    public void add(T object) {
        if (set.contains(object)) return;
        set.add(object);
        data.add(object);
    }

    public void clear() {
        set.clear();
        data.clear();
    }

    public T get(int index) {
        return index >= 0 && index < size() ? data.get(index) : null;
    }

    public void remove(int index) {
        if (index < 0 || index >= size()) return;
        set.remove(data.get(index));
        data.remove(index);
    }

    public void remove(T object) {
        set.remove(object);
        data.remove(object);
    }

    public ArrayList<T> getData() {
        return data;
    }
}
