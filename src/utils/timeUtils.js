import { parseISO } from "date-fns";

export function formatTime(time) {
  if(!time) return null;
  // Convert the string to a Date object
  const date = typeof time !== 'object' ? new Date(time) : time;

  // Create a DateTimeFormat object with the desired format
  const formatter = new Intl.DateTimeFormat('en-GB', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      hour12: false,
  });

  // Format the date and time
  const formattedDateTime = formatter.format(date);

  // Replace '/' with '.' and remove commas
  return formattedDateTime.replace(/\//g, '.').replace(/,/g, '');
}

export function getCurrentTime() {
    return formatTime(new Date().toISOString());
}