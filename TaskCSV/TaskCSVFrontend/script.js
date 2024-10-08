const apiUrl = 'http://localhost:5254/Person';

async function fetchData() {
    const response = await fetch(apiUrl);
    const data = await response.json();
    return data;
}

function displayData(people) {
    const tableBody = document.getElementById('tableBody');
    tableBody.innerHTML = '';

    people.forEach(person => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td contenteditable="true" data-field="name">${person.name}</td>
            <td contenteditable="true" data-field="dateOfBirth">${new Date(person.dateOfBirth).toLocaleDateString('ru-RU')}</td>
            <td contenteditable="true" data-field="married">${person.married}</td>
            <td contenteditable="true" data-field="phone">${person.phone}</td>
            <td contenteditable="true" data-field="salary">${person.salary}</td>
            <td>
                <button onclick="updatePerson('${person.phone}')">Save editing</button>
                <button onclick="deletePerson('${person.phone}')">delete</button>
            </td>
        `;
        tableBody.appendChild(row);
    });
}

function filterData(people) {
    const searchInput = document.getElementById('searchInput').value.toLowerCase();
    return people.filter(person =>
        person.name.toLowerCase().includes(searchInput) ||
        String(person.dateOfBirth).toLowerCase().includes(searchInput) ||
        String(person.married).toLowerCase().includes(searchInput) ||
        person.phone.toLowerCase().includes(searchInput) ||
        String(person.salary).toLowerCase().includes(searchInput)
    );
}

function sortTable(column) {
    const tableBody = document.getElementById('tableBody');
    const rows = Array.from(tableBody.rows);
    let sortedRows;

    if (column === 'Salary') {
        sortedRows = rows.sort((a, b) => parseFloat(a.cells[4].innerText) - parseFloat(b.cells[4].innerText));
    } else if (column === 'DateOfBirth') {
        sortedRows = rows.sort((a, b) => new Date(a.cells[1].innerText) - new Date(b.cells[1].innerText));
    } else {
        sortedRows = rows.sort((a, b) => a.cells[0].innerText.localeCompare(b.cells[0].innerText));
    }

    tableBody.innerHTML = '';
    sortedRows.forEach(row => tableBody.appendChild(row));
}

async function updatePerson(phone) {
    const row = Array.from(document.getElementById('tableBody').rows).find(r => r.cells[3].innerText === phone);
    if (!row) return;

    const dateParts = row.cells[1].innerText.trim().split('.');
    const formattedDate = `${dateParts[2]}-${dateParts[1]}-${dateParts[0]}`;

    const updatedPerson = {
        name: row.cells[0].innerText.trim(),
        dateOfBirth: new Date(Date.parse(formattedDate)).toISOString(),
        married: row.cells[2].innerText.trim() === 'true' ? true : false,
        phone: row.cells[3].innerText.trim(),
        salary: parseFloat(row.cells[4].innerText.trim())
    };

    const response = await fetch(`${apiUrl}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(updatedPerson)
    });

    const people = await fetchData();
    displayData(people);
}

async function deletePerson(phone) {
    const response = await fetch(`${apiUrl}/${phone}`, {
        method: 'DELETE'
    });

    const people = await fetchData();
    displayData(people);
}

async function uploadCSV(file) {
    const formData = new FormData();
    formData.append('file', file);

    const response = await fetch(`${apiUrl}`, {
        method: 'POST',
        body: formData
    });

    const people = await fetchData();
    displayData(people);
}

async function main() {
    const people = await fetchData();
    displayData(people);

    document.getElementById('searchInput').addEventListener('input', () => {
        const filteredPeople = filterData(people);
        displayData(filteredPeople);
    });

    document.getElementById('uploadButton').addEventListener('click', () => {
        const fileInput = document.getElementById('fileInput');
        const file = fileInput.files[0];
        uploadCSV(file);
    });
}

main();
