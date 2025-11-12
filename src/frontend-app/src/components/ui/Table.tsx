import type {
    HTMLAttributes,
    ReactNode,
    TableHTMLAttributes,
    ThHTMLAttributes,
    TdHTMLAttributes
} from "react";

interface TableProps extends TableHTMLAttributes<HTMLTableElement> {
    children: ReactNode;
}

interface TableSectionProps extends HTMLAttributes<HTMLTableSectionElement> {
    children: ReactNode;
}

interface TableRowProps extends HTMLAttributes<HTMLTableRowElement> {
    children: ReactNode;
}

interface TableHeadCellProps extends ThHTMLAttributes<HTMLTableCellElement> {
    children: ReactNode;
}

interface TableCellProps extends TdHTMLAttributes<HTMLTableCellElement> {
    children: ReactNode;
}

export const Table = ({ className = "", children, ...props }: TableProps) => (
    <table
        className={`min-w-full divide-y divide-surface-light-border dark:divide-surface-dark-border ${className}`}
        {...props}
    >
        {children}
    </table>
);

export const TableHeader = ({ className = "", children, ...props }: TableSectionProps) => (
    <thead
        className={`bg-surface-light dark:bg-surface-dark ${className}`}
        {...props}
    >
        {children}
    </thead>
);

export const TableBody = ({ className = "", children, ...props }: TableSectionProps) => (
    <tbody
        className={`divide-y divide-surface-light-border dark:divide-surface-dark-border bg-white dark:bg-surface-dark ${className}`}
        {...props}
    >
        {children}
    </tbody>
);

export const TableRow = ({ className = "", children, ...props }: TableRowProps) => (
    <tr className={className} {...props}>
        {children}
    </tr>
);

export const TableHead = ({ className = "", children, ...props }: TableHeadCellProps) => (
    <th
        className={`px-4 py-3 text-left text-sm font-semibold text-gray-600 dark:text-gray-300 ${className}`}
        {...props}
    >
        {children}
    </th>
);

export const TableCell = ({ className = "", children, ...props }: TableCellProps) => (
    <td
        className={`px-4 py-3 text-sm text-gray-700 dark:text-gray-200 ${className}`}
        {...props}
    >
        {children}
    </td>
);
