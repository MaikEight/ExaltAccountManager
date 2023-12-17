import { parseISO } from "date-fns";

export function formatTime(time) {
    if(!time) return null;
  // Convert the string to a Date object
  const date = parseISO(time);

  // Get the user's locale (you might get this information from user preferences)
  const userLocale = navigator.language || 'en-US';

  // Format the date and time with zero-based hours
  const formattedDateTime = date.toLocaleDateString(userLocale, {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
  }) + ', ' + date.toLocaleTimeString(userLocale, {
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
    hour12: false, // Ensure 24-hour format
  });

  return formattedDateTime.replace(',', '');
}