// Version Function - Complete
function getVersion() {
    fetch('/api/GetVersion')
        .then(response1 => response1.text())
        .then(data1 => {
            document.getElementById('version_display_obj').innerText = `Version: ${data1}`;
        })
        .catch(error1 => {
            console.error('Error fetching version:', error1);
        });
}

getVersion()