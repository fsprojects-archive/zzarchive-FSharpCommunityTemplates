<div class="nine.columns.centered">
    <p class="row">
        <input id="createContact" type="button" class="small button radius" value="Create New" />
    </p>
    <div class="row">
        <div class="five columns header">FirstName</div>
        <div class="five columns header">LastName</div>
        <div class="two columns header">Phone</div>
    </div>
    
    <% for (var i = 0; i < data.length; i++) { %>
        <div class="row">
            <div class="five columns"><%= data[i].firstName  %></div>
            <div class="five columns"><%= data[i].lastName  %></div>
            <div class="two columns"><%= data[i].phone  %></div>
        </div>
    <% } %>
</div>