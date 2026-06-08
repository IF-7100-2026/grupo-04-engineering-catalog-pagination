// Página actual que se está mostrando en la tabla.
let currentPage = 0;

// Referencias a los elementos principales del HTML.
const pageSizeSelect = document.getElementById("pageSize");
const previousButton = document.getElementById("previousButton");
const nextButton = document.getElementById("nextButton");
const partsTableBody = document.getElementById("partsTableBody");
const pageInfo = document.getElementById("pageInfo");
const totalInfo = document.getElementById("totalInfo");
const timeInfo = document.getElementById("timeInfo");
const message = document.getElementById("message");
const materialFilter = document.getElementById("materialFilter");
const partTypeFilter = document.getElementById("partTypeFilter");
const weightFromFilter = document.getElementById("weightFromFilter");
const weightToFilter = document.getElementById("weightToFilter");
const sizeFromFilter = document.getElementById("sizeFromFilter");
const sizeToFilter = document.getElementById("sizeToFilter");
const manufactureDateFromFilter = document.getElementById("manufactureDateFromFilter");
const manufactureDateToFilter = document.getElementById("manufactureDateToFilter");
const registrationFromFilter = document.getElementById("registrationFromFilter");
const registrationToFilter = document.getElementById("registrationToFilter");
const descriptionFilter = document.getElementById("descriptionFilter");
const searchButton = document.getElementById("searchButton");
const toIsoDateTime = (v) => v ? new Date(v).toISOString() : null;

// Cuando la página termina de cargar, se consulta la primera página de datos.
document.addEventListener("DOMContentLoaded", () => {
    loadParts();

    // Si cambia la cantidad de registros por página, se vuelve a la primera página.
    pageSizeSelect.addEventListener("change", () => {
        currentPage = 0;
        loadParts();
    });

    // Carga la página anterior si no estamos en la primera.
    previousButton.addEventListener("click", () => {
        if (currentPage > 0) {
            currentPage--;
            loadParts();
        }
    });

    // Carga la siguiente página.
    nextButton.addEventListener("click", () => {
        currentPage++;
        loadParts();
    });

    searchButton.addEventListener("click", () => {
        currentPage = 0;
        loadParts();
    });
});

// Consulta el endpoint paginado del backend.
async function loadParts() {

    const pageSize = pageSizeSelect.value;

    const params = new URLSearchParams();

    params.append("page", currentPage);
    params.append("size", pageSize);

    if (materialFilter.value.trim() !== "") {
        params.append("material", materialFilter.value.trim());
    }

    if (partTypeFilter.value.trim() !== "") {
        params.append("partType", partTypeFilter.value.trim());
    }

    if (weightFromFilter.value !== "") {
        params.append("weightFrom", weightFromFilter.value);
    }

    if (weightToFilter.value !== "") {
        params.append("weightTo", weightToFilter.value);
    }

    if (sizeFromFilter.value !== "") {
        params.append("sizeFrom", sizeFromFilter.value);
    }

    if (sizeToFilter.value !== "") {
        params.append("sizeTo", sizeToFilter.value);
    }

    if (manufactureDateFromFilter.value !== "") {
        params.append("manufactureDateFrom", manufactureDateFromFilter.value);
    }

    if (manufactureDateToFilter.value !== "") {
        params.append("manufactureDateTo", manufactureDateToFilter.value);
    }

    if (registrationFromFilter.value) {
        params.append("registrationFrom", toIsoDateTime(registrationFromFilter.value));
    }

    if (registrationToFilter.value) {
        params.append("registrationTo", toIsoDateTime(registrationToFilter.value));
    }

    if (descriptionFilter.value.trim() !== "") {
        params.append("descriptionContains", descriptionFilter.value.trim());
    }

    const endpoint = `/api/parts?${params.toString()}`;

    setLoadingState(true);

    try {
        const response = await fetch(endpoint);
        const data = await response.json();

        if (!response.ok) {
            showMessage(data.message || "An error occurred while loading the catalog.");
            return;
        }

        renderTable(data.content);
        updatePaginationInfo(data);
        showMessage("");
    }
    catch (error) {
        showMessage("Could not connect to the backend API.");
    }
    finally {
        setLoadingState(false);
    }
}

// Dibuja en la tabla los registros recibidos desde el backend.
function renderTable(parts) {
    partsTableBody.innerHTML = "";

    parts.forEach(part => {
        const row = document.createElement("tr");

        row.innerHTML = `
            <td>${part.partId}</td>
            <td>${part.partType}</td>
            <td>${part.material}</td>
            <td>${part.manufactureDate}</td>
            <td>${part.registrationTimestamp}</td>
            <td>${part.weightKg}</td>
            <td>${part.sizeMeters}</td>
            <td class="description-cell">${truncateText(part.longDescription, 90)}</td>
        `;

        partsTableBody.appendChild(row);
    });
}

// Actualiza la información de paginación mostrada al usuario.
function updatePaginationInfo(data) {
    pageInfo.textContent = `Page ${data.page + 1} of ${data.totalPages}`;
    totalInfo.textContent = `Total records: ${data.totalElements}`;
    timeInfo.textContent = `Response time: ${data.responseTimeMs} ms`;

    previousButton.disabled = !data.hasPrevious;
    nextButton.disabled = !data.hasNext;
}

// Bloquea o desbloquea los botones mientras se cargan los datos.
function setLoadingState(isLoading) {
    previousButton.disabled = isLoading;
    nextButton.disabled = isLoading;
    message.textContent = isLoading ? "Loading records..." : "";
}

// Muestra mensajes simples de error o carga.
function showMessage(text) {
    message.textContent = text;
}

// Recorta descripciones largas para que la tabla sea más legible.
function truncateText(text, maxLength) {
    if (!text || text.length <= maxLength) {
        return text;
    }

    return text.substring(0, maxLength) + "...";
}