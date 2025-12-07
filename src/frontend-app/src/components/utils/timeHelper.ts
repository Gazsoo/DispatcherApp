export const formatDateTime = (value?: Date) => {
    if (!value) return '—';
    try {
        const d = value instanceof Date ? value : new Date(value);
        if (isNaN(d.getTime())) return '—';
        return d.toLocaleString();
    } catch {
        return '—';
    }
};