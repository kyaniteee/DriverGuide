window.createObjectURL = (bytes, mimeType) => {
    var blob = new Blob([new Uint8Array(bytes)], { type: mimeType });
    return URL.createObjectURL(blob);
};

function createObjectURL(byteArray, contentType) {
    const blob = new Blob([byteArray], { type: contentType });
    return URL.createObjectURL(blob);
}