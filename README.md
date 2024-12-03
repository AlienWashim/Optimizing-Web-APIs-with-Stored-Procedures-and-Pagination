# Optimizing Web APIs with Stored Procedures and Pagination

## What is Pagination?
Pagination is the process of dividing a large dataset into smaller, manageable chunks or pages. When fetching data from a database, instead of retrieving all records at once, pagination ensures only a subset of records is returned, often controlled by parameters like pageNumber and pageSize.

## Why is Pagination Important?
* **Performance**: Fetching large amounts of data at once can slow down the application and increase memory consumption. Pagination helps reduce load times by fetching only the required records.

* **Scalability**: As the database grows, pagination ensures the application can handle increased data volume without degrading performance.

* **User Experience**: By loading smaller amounts of data per page, users can quickly view content without delays. It also makes it easier to navigate large datasets.

## When Should You Use Pagination?
* **Large Datasets**: Pagination is crucial when working with tables or data collections that could have thousands or millions of rows.
* **Search Results**: When displaying search results, pagination helps to manage how results are shown to the user, providing clear and quick navigation.
* **Reports and Analytics**: In scenarios where large reports are generated (e.g., transaction logs or logs), pagination allows users to access a specific subset of the data instead of waiting for the entire report to load.

## Real-Life Example of Pagination:
Imagine an e-commerce website where users can browse products. If the database contains thousands of products, loading all the products at once would be slow and inefficient. Instead, the website loads a fixed number of products per page (e.g., 10 or 20). Users can then navigate through pages to view the rest of the products without overloading the server or slowing down the user experience.

## Stored Procedure Code for Pagination (SQL)
```sql
-- Stored Procedure to Get Paginated Persons
CREATE PROCEDURE GetPaginatedPersons
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SELECT * FROM Persons
    ORDER BY Id
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
```

## C# Controller Method for Paginated Data
```csharp
 // Get Paginated Persons
 [HttpGet]
 public ActionResult<IEnumerable<Person>> GetPaginatedPersons(int pageNumber = 1, int pageSize = 10)
 {
     var persons = _databaseServices.GetPaginatedPersons(pageNumber, pageSize);
     return Ok(persons);
 }
```

## C# DatabaseService Method for Paginated Data
```csharp
// Get Paginated Persons
public List<Person> GetPaginatedPersons(int pageNumber, int pageSize)
{
    List<Person> persons = new List<Person>();

    using (var connection = new SqlConnection(_connectionString))
    {
        connection.Open();
        using (var command = new SqlCommand("GetPaginatedPersons", connection))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    persons.Add(new Person
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Salary = reader.GetDecimal(2)
                    });
                }
            }
        }
    }

    return persons;
}
```

## Conclusion
Pagination plays a critical role in improving the performance, scalability, and user experience of your Web API. By using stored procedures for paginated data retrieval, you can optimize database queries, enhance security, and reduce the overhead of fetching large datasets. This approach ensures that your Web API remains responsive even as data volume grows.

By combining stored procedures and pagination, you build more efficient, maintainable, and scalable applications that are ready to handle complex data operations.
