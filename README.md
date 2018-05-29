# Web API content negociation demo
Demo on how to support different content negociation formats

Supported accept header --> `Accept: application/json`

Added XML formatter for xml content negociation --> `Accept: text/xml`

Implemented custom content negociation of CSV format --> `Accept: text/csv`

## Create your own custom formatters

You can create your own text based formatters by inheriting from `TextOutputFormatter` and build a parser. I have converted `"ToDo"` model class into comma separated string values.

```cs
public class CsvFormatter : TextOutputFormatter
  {
      public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, 
        Encoding selectedEncoding)
      {
          var response = context.HttpContext.Response;
          var buffer = new StringBuilder();
          if (context.Object is IEnumerable<TodoItem>)
          {
              foreach(var todoItem in (IEnumerable<TodoItem>)context.Object)
              {
                  FormatCsv(buffer, todoItem);
              }
          }
          else
          {
              FormatCsv(buffer, (TodoItem)context.Object);
          }
          using (var writer = context.WriterFactory(response.Body, selectedEncoding))
          {
              return writer.WriteAsync(buffer.ToString());
          }
      }

      private static void FormatCsv(StringBuilder buffer, TodoItem item)
      {
          buffer.AppendLine($"{item.Id},\"{item.Name}\",{item.IsComplete}");
      }
  }

```

Add this newly created CsvFormatter instance to the list of available formatters in Startup.cs file. And match the formatter with media type 'text/csv'.

```cs
  options.FormatterMappings.SetMediaTypeMappingForFormat(
    "csv", MediaTypeHeaderValue.Parse("text/csv"));
  options.OutputFormatters.Add(new CsvFormatter());
```




 
