// Create a blob URL from a byte array
window.createObjectURL = function (bytes, mimeType) {
    console.log("Creating blob URL with mime type:", mimeType);

    // Ensure bytes is a valid array
    if (!bytes || bytes.length === 0) {
        console.error("createObjectURL: byte array is null or empty.");
        return null;
    }

    try {
        const byteArray = new Uint8Array(bytes);
        const blob = new Blob([byteArray], { type: mimeType });
        const url = URL.createObjectURL(blob);
        console.log("Successfully created blob URL:", url);
        return url;
    } catch (e) {
        console.error("Error creating blob:", e);
        return null;
    }
};

// Revoke a blob URL to free memory
window.revokeObjectURL = function (url) {
    if (url && url.startsWith("blob:")) {
        console.log("Revoking blob URL:", url);
        URL.revokeObjectURL(url);
    }
};

// Log that the script has been loaded
console.log("videoUtils.js loaded successfully");