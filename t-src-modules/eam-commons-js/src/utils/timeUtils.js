
function formatTime(time, dateOnly = false) {
    if (!time) return null;
    // Convert the string to a Date object
    const date = typeof time !== 'object' ? new Date(time) : time;

    // Create a DateTimeFormat object with the desired format
    const formatter = new Intl.DateTimeFormat('en-GB', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        ...(!dateOnly ? {
            hour: '2-digit',
            minute: '2-digit',
            hour12: false,
        } : {})
    });

    // Format the date and time
    const formattedDateTime = formatter.format(date);

    // Replace '/' with '.' and remove commas
    return formattedDateTime.replace(/\//g, '.').replace(/,/g, '');
}

function getCurrentTime() {
    return formatTime(new Date().toISOString());
}

/**
 * Returns a string representing the relative time from now to the given date.
 * e.g., "5 minutes ago", "in 2 hours"
 * Up to 12 months in the past or future, otherwise returns "a long time ago" or "in a long time"
 * @param {Date} date 
 */
function getRelativeTimeString(date) {
    const now = new Date();
    const diff = date - now; // difference in milliseconds
    const absDiff = Math.abs(diff);

    const minute = 60 * 1000;
    const hour = 60 * minute;
    const day = 24 * hour;
    const month = 30 * day;

    let value, unit;

    if (absDiff < minute) {
        value = Math.round(absDiff / 1000);
        unit = 'second';
    } else if (absDiff < hour) {
        value = Math.round(absDiff / minute);
        unit = 'minute';
    } else if (absDiff < day) {
        value = Math.round(absDiff / hour);
        unit = 'hour';
    } else if (absDiff < month) {
        value = Math.round(absDiff / day);
        unit = 'day';
    } else if (absDiff < 12 * month) {
        value = Math.round(absDiff / month);
        unit = 'month';
    } else {
        return diff < 0 ? 'a long time ago' : 'in a long time';
    }

    if (value !== 1) {
        unit += 's'; // pluralize unit
    }

    return diff < 0 ? `${value} ${unit} ago` : `in ${value} ${unit}`;
}

export {
    formatTime,
    getCurrentTime,
    getRelativeTimeString
};