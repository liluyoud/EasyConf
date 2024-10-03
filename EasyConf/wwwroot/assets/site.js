function reloadPage() {
    location.reload(true);
}

function blazorDownloadFile(fileName, content) {
    const element = document.createElement('a');
    const file = new Blob([content], { type: 'text/plain' });
    element.href = URL.createObjectURL(file);
    element.download = fileName;
    document.body.appendChild(element); // Requerido para Firefox
    element.click();
    document.body.removeChild(element);
}

function copyTextToClipboard(text) {
    navigator.clipboard.writeText(text).then(function () {
        console.log('Texto copiado para o clipboard: ' + text);
    }).catch(function (error) {
        console.error('Erro ao copiar o texto: ', error);
    });
}