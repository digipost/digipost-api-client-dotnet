---
title: Archive
identification: archive
layout: default
---

The archive API makes it possible for an organisation to manage documents in archives. These files are kept in 
separate archives, and the files belong to the sender organisation.

### Archive a file

Let's say you want to archive two documents eg. an invoice and an attachment and
you want to have some kind of reference to both documents. You can do that
by describing the two documents with `ArchiveDocument`. Then you need to create an archive
and add the documents to the archive. In the following example we use a default archive.
`readFileFromDisk` is just made up function to showcase how you might read a file to a `byte[]`
_`sender` is your sender, the one you use to create the client instance, if you are not a broker._ For broker see below.

```csharp
var archive = new Archive.Archive(sender, new List<ArchiveDocument>
{
    new ArchiveDocument(Guid.NewGuid(), "invoice_123123.pdf", "pdf","application/psd", readFileFromDisk("invoice_123123.pdf")),
    new ArchiveDocument(Guid.NewGuid(), "attachment_123123.pdf", "pdf","application/psd", readFileFromDisk("attachment_123123.pdf"))
});

var savedArchive = await client.GetArchive().ArchiveDocuments(archive);
```

You can name the archive if you wish to model your data in separate archives

```csharp
var archive = new Archive.Archive(sender, new List<ArchiveDocument>(), "MyArchiveName");
```

## Get a list of archives
An organisation can have many archives, or just the default unnamed archive. That is up to
your design wishes. To get a list of the archives for a given Sender, you can do this:

```csharp
IEnumerable<Archive.Archive> fetchedArchives = await client.GetArchive().FetchArchives();
```

## Iterate documents in an archive
You _can_ get content of an archive with paged requests. Under is an example of how to iterate
an archive. However, its use is strongly discouraged because it leads to the idea that
an archive can be iterated. We expect an archive to possibly reach many million rows so the iteration
will possibly give huge loads. On the other hand being able to dump all data is a necessary feature of any archive.

_Please use fetch document by UUID or referenceID instead to create functionality on top of the archive._
You should on your side know where and how to get a document from an archive. You do this by knowing where
you put a file you want to retrieve.

```csharp
var current = (await client.GetArchive().FetchArchives()).First();
var documents = new List<ArchiveDocument>();

while (current.HasMoreDocuments())
{
    var fetchArchiveDocuments = client.GetArchive().FetchArchiveDocuments(current.GetNextDocumentsUri()).Result;
    documents.AddRange(fetchArchiveDocuments.ArchiveDocuments);
    current = fetchArchiveDocuments;
}

// documents now have all ArchiveDocuments in the archive (not the actual bytes, just the meta data)
```

In c# 8 there is a language feature to produce a `IAsyncEnumerable` so that you can implement a stream of 
archive documents. But this has not been implemented in this client library since we are on dotnetstandard2.0.
For each iteration above you will get 100 documents.

## Archive Document attributes
You can add optional attributes to documents. An attribute is a key/val string-dictionary that describe documents. You can add
up to 15 attributes per archive document. The attribute's key and value are case sensitive.

```csharp
var archiveDocument = new ArchiveDocument(Guid.NewGuid(), "invoice_123123.pdf", "pdf", "application/psd", readFileFromDisk("invoice_123123.pdf"))
{
    Attributes = {["invoicenumber"] = "123123"}
};
```

The attributes can be queried, so that you can get an iterable list of documents.

```csharp
var archive = (await client.GetArchive().FetchArchives()).First();
var searchBy = new Dictionary<string, string>
{
    ["key"] = "val"
};

var fetchArchiveDocuments = client.GetArchive().FetchArchiveDocuments(archive.GetNextDocumentsUri(searchBy)).Result;
var documents = fetchArchiveDocuments.ArchiveDocuments;

// documents now have all ArchiveDocuments in the archive that has invoicenumber=123123
```

We recommend that the usage of attributes is made such that the number of results for a query on attributes
is less than 100. If you still want that, it's ok, but you need to iterate the pages to get all the results
like we did in the example where we fetch all documents in an archive.

```csharp
var current = (await client.GetArchive().FetchArchives()).First();
var documents = new List<ArchiveDocument>();

var searchBy = new Dictionary<string, string>
{
    ["key"] = "val"
};

while (current.HasMoreDocuments())
{
    var fetchArchiveDocuments = client.GetArchive().FetchArchiveDocuments(current.GetNextDocumentsUri(searchBy)).Result;
    documents.AddRange(fetchArchiveDocuments.ArchiveDocuments);
    current = fetchArchiveDocuments;
}

// documents now have all ArchiveDocuments in the archive that has invoicenumber=123123
```

## Get documents by referenceID

You can retrieve a set of documents by a given referenceID. You will then get the documents listed in their respective
archives in return. ReferenceId is a string you can add to documents. The idea is that you might want to model 
documents in different archives and we able to fetch them combined. A good reference ID might be a unique
technical process ID or a conversation ID of some kind. It is thus not meant to describe the actual document
but is rather a join key of some kind.

```csharp
IEnumerable<Archive.Archive> fetchArchiveDocumentsByReferenceId = await client.GetArchive().FetchArchiveDocumentsByReferenceId("MyProcessId[No12341234]");
```

As you can see, it is a list of `Archives` containing `ArchiveDocument`;
You should choose a ReferenceId in such a way to try and keep the count of documents low so as feasible.

## Get documents by externalId or Guid/Uuid

You can retrieve a set of documents by the Guid/UUID that you give the document when you archive it. In the example above
we use `Guid.NewGuid()` to generate an uuid/guid. You can either store that random uuid in your database for
retrieval later, or you can generate a deterministic uuid based on your conventions for later retrieval.

You will get in return an instance of `Archive` which contains information on the archive the document is contained in
and the actual document. From this you can fetch the actual document.

```csharp
ArchiveDocument archiveDocument = (await client.GetArchive(sender).FetchDocumentFromExternalId(Guid.Parse("10ff4c99-8560-4741-83f0-1093dc4deb1c"))).One();
```
```csharp
Archive archive = await client.GetArchive(sender).FetchDocumentFromExternalId("MyExternalId");
```

You can store your guid that you used at upload time to fetch by `Guid`.

__Note:__
The reason for having the string and Guid as a parameter is that with the java client it is possible to create an UUID 
from a string. This is done deterministically. Under normal circumstance you should use `Guid.NewGuid()` when you 
upload an document and store this id for reference in your system. But if you want to fetch a document that you 
know is stored by another entity that uses the deterministic UUID approach with java and all you have is the
original string, then you need to use the string parameterized method.
Java generates UUID in slightly different manner compared to .NET Guid. Therefore we have implemented the 
same UUID-generation logic into the client library api. You can use the method `Guid.Parse(UuidInterop.NameUuidFromBytes("MyExternalId""))`
to create an exact equal value for a _Guid_ compared to _Uuid_.

## Get content of a document as a single-use link
You can get the actual content of a document after you have retrieved the archive document. Below is an example of how
you can achieve this with a given `ArchiveDocument`. In the resulting `ArchiveDocumentContent`, you will get a url to
the content which expires after 30 seconds.

```csharp
//First fetch the archiveDocument
ArchiveDocument archiveDocument = await client.GetArchive(sender).FetchArchiveDocument(client.GetRoot(new ApiRootUri()).GetGetArchiveDocumentsByUuidUri(Guid.Parse("10ff4c99-8560-4741-83f0-1093dc4deb1c")));

ArchiveDocumentContent archiveDocumentContent = await client.GetArchive().GetDocumentContent(archiveDocument.DocumentContentUri());
Uri uri = archiveDocumentContent.Uri;
```

## Get content of a document as a stream

In addition to a single-use link, you also have the option to retrieve the content of a document directly as a
byte stream. 

```csharp
Stream streamDocumentFromExternalId = await client.GetArchive(sender).StreamDocumentFromExternalId(Guid.Parse("10ff4c99-8560-4741-83f0-1093dc4deb1c"));
```

## Update document attributes and/or referenceId
You can add an attribute or change an attribute value, but not delete an attribute. You can however set the value
to empty string. The value of the field for referenceID can be changed as well. 

```csharp
ArchiveDocument archiveDocument = await client.GetArchive(sender).FetchDocumentFromExternalId(Guid.Parse("10ff4c99-8560-4741-83f0-1093dc4deb1c"));
archiveDocument.WithAttribute("newKey", "foobar")
    .WithReferenceId("MyProcessId[No12341234]Done");

ArchiveDocument updatedArchiveDocument = await client.GetArchive().UpdateDocument(archiveDocument, archiveDocument.GetUpdateUri());
```

## Set autoDelete time 
Sometimes you want to delete documents automatically. This can be done at upload time.

```csharp
var archive = new Archive.Archive(sender, new List<ArchiveDocument>
{
    new ArchiveDocument(Guid.NewGuid(), "invoice_123123.pdf", "pdf", "application/psd", readFileFromDisk("invoice_123123.pdf"))
        .WithDeletionTime(DateTime.Today.AddYears(5))
});

var savedArchive = await client.GetArchive().ArchiveDocuments(archive);
```

## Using archive as a broker

It is possible to be a broker for an actual sender. Most of the api described above also support
the use of SenderId to specify who you are archiving for.

eg.:
```csharp
client.GetArchive(new Sender(111111)).FetchArchives()
```

As you can see, this is typically done when you get the api part `GetArchive`. This will use the feature of Root element 
in the restful api and serve the client with url resources specific for that sender you are broker for.

At upload time, however, this is done in the actual message you send.

```csharp
var archive = new Archive.Archive(new Sender(111111), new List<ArchiveDocument>
{
    new ArchiveDocument(Guid.NewGuid(), "invoice_123123.pdf", "pdf","application/psd", readFileFromDisk("invoice_123123.pdf")),
    new ArchiveDocument(Guid.NewGuid(), "attachment_123123.pdf", "pdf","application/psd", readFileFromDisk("attachment_123123.pdf"))
});

var savedArchive = await client.GetArchive().ArchiveDocuments(archive);
```
