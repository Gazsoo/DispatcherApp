import React from 'react';
import type { UserInfoResponse } from '../../../services/web-api-client';
import { useUser } from "../../context/userContext";


const UserBadge = () => {
    const { user, userRoles } = useUser();
    const rolesList = Array.isArray(userRoles)
        ? userRoles
        : (userRoles ? [userRoles] : []);

    {/* User info */ }
    return (
        <div className="flex items-center gap-3 px-3 py-3 rounded-lg bg-surface-light-card dark:bg-surface-dark-card border border-surface-light-border dark:border-surface-dark-border">
            <div className="flex h-10 w-10 items-center justify-center rounded-full bg-accent/20 text-accent font-semibold">
                {(() => {
                    const fn = user?.firstName?.trim() || '';
                    const ln = user?.lastName?.trim() || '';
                    const initials = `${fn?.[0] ?? ''}${ln?.[0] ?? ''}` || (user?.email?.[0]?.toUpperCase() ?? '?');
                    return initials.toUpperCase();
                })()}
            </div>
            <div className="min-w-0">
                <div className="font-medium truncate">
                    {user?.firstName || user?.lastName ? `${user?.firstName ?? ''} ${user?.lastName ?? ''}`.trim() : (user?.email ?? 'Unknown user')}
                </div>
                <div className="text-xs text-gray-600 dark:text-gray-400 truncate">
                    {(rolesList.length > 0 ? rolesList.join(', ') : (user?.role ?? '')) || '—'}
                </div>
            </div>
        </div>
    );
};
export default UserBadge;