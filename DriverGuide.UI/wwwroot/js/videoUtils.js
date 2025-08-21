window.createObjectURL = (bytes, mimeType) => {
    var blob = new Blob([new Uint8Array(bytes)], { type: mimeType });
    return URL.createObjectURL(blob);
};