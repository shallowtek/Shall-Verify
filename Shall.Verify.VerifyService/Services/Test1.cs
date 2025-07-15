public class Document
{

    public string Title { get; set; }

    public string Content { get; set; }

    public void Save()
    {

        File.WriteAllText("doc.txt", Content);

    }

    public void Print()
    {

        Console.WriteLine("Printing: " + Content);

    }

    public void UploadToCloud()
    {

        Console.WriteLine("Uploading to cloud...");

        // Simulate cloud API call

    }

}

public class CloudService
{

    public void SyncDocument(Document doc)
    {

        if (doc.Content.Length > 10000)

            throw new Exception("Too large to sync");

        Console.WriteLine("Syncing " + doc.Title);

    }

}

public class DocumentProcessor
{

    private CloudService _cloudService = new CloudService();

    public void ProcessDocument(Document doc)
    {

        if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)

            return;

        doc.Save();

        doc.Print();

        doc.UploadToCloud();

        _cloudService.SyncDocument(doc);

    }

}

