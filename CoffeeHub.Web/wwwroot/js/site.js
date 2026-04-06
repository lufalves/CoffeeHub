// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Session timeout handling
(function () {
    'use strict';

    // Configuration
    const SESSION_TIMEOUT_MS = 14 * 24 * 60 * 60 * 1000; // 14 days from cookie settings
    const WARNING_TIME_MS = 2 * 60 * 1000; // Show warning 2 minutes before expiration
    const CHECK_INTERVAL_MS = 30 * 1000; // Check every 30 seconds
    const EXTEND_SESSION_URL = '/Account/ExtendSession'; // Endpoint to call to extend session

    let lastActivity = Date.now();
    let warningTimeout = null;
    let checkInterval = null;
    let isWarningShown = false;

    // Get elements
    const warningElement = document.getElementById('session-timeout-warning');
    if (!warningElement) return; // Exit if element doesn't exist

    const staySignedInButton = warningElement.querySelector('button');

    // Update last activity timestamp
    function updateLastActivity() {
        lastActivity = Date.now();
    }

    // Check if session is about to expire
    function checkSessionStatus() {
        const timeSinceLastActivity = Date.now() - lastActivity;
        const timeUntilExpiration = SESSION_TIMEOUT_MS - timeSinceLastActivity;
        
        // If less than warning time remains and warning isn't already shown
        if (timeUntilExpiration <= WARNING_TIME_MS && !isWarningShown) {
            showWarning();
        } 
        // If more than warning time remains and warning is shown, hide it
        else if (timeUntilExpiration > WARNING_TIME_MS && isWarningShown) {
            hideWarning();
        }
    }

    // Show the warning
    function showWarning() {
        warningElement.style.display = 'block';
        isWarningShown = true;
    }

    // Hide the warning
    function hideWarning() {
        warningElement.style.display = 'none';
        isWarningShown = false;
    }

    // Extend session by making a request to the server
    function extendSession() {
        fetch('/Account/ExtendSession', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getAntiForgeryToken()
            },
            credentials: 'include'
        })
        .then(response => {
            if (response.ok) {
                // Reset the timer on successful extension
                updateLastActivity();
                hideWarning();
            }
        })
        .catch(error => {
            console.error('Failed to extend session:', error);
        });
    }

    // Get anti-forgery token from the input field injected in _Layout.cshtml
    function getAntiForgeryToken() {
        const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
        if (tokenInput) {
            return tokenInput.value;
        }
        
        // Fallback to cookie
        const cookieMatch = document.cookie.match(new RegExp('(^| )\\s*RequestVerificationToken\\s*=\\s*([^;]+)'));
        if (cookieMatch) {
            return decodeURIComponent(cookieMatch[2]);
        }
        
        return '';
    }

    // Event listeners for user activity
    document.addEventListener('mousemove', updateLastActivity);
    document.addEventListener('keypress', updateLastActivity);
    document.addEventListener('click', updateLastActivity);
    document.addEventListener('scroll', updateLastActivity);

    // Button click handler
    if (staySignedInButton) {
        staySignedInButton.addEventListener('click', extendSession);
    }

    // Initialize
    function initialize() {
        // Set initial activity time
        updateLastActivity();
        
        // Start checking session status
        checkInterval = setInterval(checkSessionStatus, CHECK_INTERVAL_MS);
        
        // Initial check
        checkSessionStatus();
    }

    // Start when DOM is loaded
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initialize);
    } else {
        initialize();
    }

    // Cleanup on page unload
    window.addEventListener('beforeunload', () => {
        if (warningTimeout) clearTimeout(warningTimeout);
        if (checkInterval) clearInterval(checkInterval);
    });
})();
