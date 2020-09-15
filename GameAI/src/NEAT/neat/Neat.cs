public class Neat
{
    private readonly double C1 = 1;
    private readonly double C2 = 1;
    private readonly double C3 = 1;
    private int input_size;
    private int max_clients;
    private int MAX_NODES = (int) Math.pow(2, 20);
    private int output_size;

    public Neat(int input_size, int output_size, int clients)
    {
        reset(input_size, output_size, clients);
        all_connections = new HashMap<>();
        all_nodes = new RandomHashSet<>();
    }

    public static final
    private final
    private final
    private final
    private final HashMap<ConnectionGene, ConnectionGene>all_connections;
    private final RandomHashSet<NodeGene>all_nodes;

    public static ConnectionGene getConnection(ConnectionGene con)
    {
        var c = new ConnectionGene(con.getFrom(), con.getTo());
        c.setWeight(con.getWeight());
        c.setEnabled(con.isEnabled());
        return c;
    }

    public static void main(String[] args)
    {
        var neat = new Neat(2, 1, 100);

        var in1 = neat.getNode(1);
        var in2 = neat.getNode(2);
        var out1 = neat.getNode(3);

        var con11 = neat.getConnection(in1, out1);
        var con12 = neat.getConnection(in2, out1);

        System.out.println(con11.getInnovation_number());
        System.out.println(con12.getInnovation_number());


        var con11_2 = neat.getConnection(in1, out1);
        con11_2.setWeight(3);

        System.out.println(con11_2.getWeight());

        //Genome g = neat.empty_genome();
        //System.out.println(g.getNodes().size());
    }

    public Genome empty_genome()
    {
        var g = new Genome(this);
        for (var i = 0; i < input_size + output_size; i++) g.getNodes().add(getNode(i + 1));
        return g;
    }

    public void reset(int input_size, int output_size, int clients)
    {
        this.input_size = input_size;
        this.output_size = output_size;
        max_clients = clients;

        all_connections.clear();
        all_nodes.clear();

        for (var i = 0; i < input_size; i++)
        {
            var n = getNode();
            n.setX(0.1);
            n.setY((i + 1) / (double) (input_size + 1));
        }

        for (var i = 0; i < output_size; i++)
        {
            var n = getNode();
            n.setX(0.9);
            n.setY((i + 1) / (double) (output_size + 1));
        }
    }

    public ConnectionGene getConnection(NodeGene node1, NodeGene node2)
    {
        var connectionGene = new ConnectionGene(node1, node2);

        if (all_connections.containsKey(connectionGene))
        {
            connectionGene.setInnovation_number(all_connections.get(connectionGene).getInnovation_number());
        }
        else
        {
            connectionGene.setInnovation_number(all_connections.size() + 1);
            all_connections.put(connectionGene, connectionGene);
        }

        return connectionGene;
    }

    public NodeGene getNode()
    {
        var n = new NodeGene(all_nodes.size() + 1);
        all_nodes.add(n);
        return n;
    }

    public NodeGene getNode(int id) { return id <= all_nodes.size() ? all_nodes.get(id - 1) : getNode(); }

    public int getOutput_size() { return output_size; }

    public int getInput_size() { return input_size; }

    public double getC1() { return C1; }

    public double getC2() { return C2; }

    public double getC3() { return C3; }
}