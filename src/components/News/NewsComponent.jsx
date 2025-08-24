

import { Box, Typography, Button, Paper, Chip, Divider } from '@mui/material';
import { useTheme } from '@emotion/react';
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import ComponentBox from '../ComponentBox';
import StyledButton from '../StyledButton';
import * as Icons from "@mui/icons-material";
import useStartupPopups from '../../hooks/useStartupPopups';
import usePopups from '../../hooks/usePopups';
import NewsCard from './NewsCard';

function NewsRenderer({ content, onAction }) {
    const theme = useTheme();
    const navigate = useNavigate();
    const { showPopup } = usePopups();
    const { changelogPopups } = useStartupPopups();

    const handleAction = (action) => {
        if (onAction) {
            onAction(action);
            return;
        }

        switch (action.type) {
            case 'navigate':
                navigate(action.path);
                break;
            case 'external_link':
                // Event is handled by the anchor tag
                break;
            case 'show_changelog':
                let changelog = changelogPopups.find((popup) => popup.version === action.version);
                changelog = changelog || changelogPopups[changelogPopups.length - 1];
                if (changelog) {
                    showPopup({
                        ...changelog,
                        onClose: () => null,
                    });
                }
                break;
            default:
                console.warn('Unknown action type:', action.type);
        }
    };

    const renderElement = (element, key = 0) => {
        if (!element) return null;

        const { type, props = {}, children, content, src, alt, action } = element;

        const validateProps = () => {
            // Required props validation
            if (type === 'image' && (!src || !alt)) {
                console.warn('Image element requires src and alt properties');
                return false;
            }

            if (type === 'button' && !content) {
                console.warn('Button element requires content');
                return false;
            }

            // Security validation - prevent XSS and malicious content
            if (src && !isValidImageUrl(src)) {
                console.warn('Invalid or potentially dangerous image URL:', src);
                return false;
            }

            // Validate action URLs for buttons/links
            if (action?.url && !isValidActionUrl(action.url)) {
                console.warn('Invalid or potentially dangerous action URL:', action.url);
                return false;
            }

            // Validate content for potential script injection
            if (content && typeof content === 'string' && containsDangerousContent(content)) {
                console.warn('Content contains potentially dangerous elements:', content);
                return false;
            }

            // Validate style properties for CSS injection
            if (props.sx && containsDangerousStyles(props.sx)) {
                console.warn('Style properties contain potentially dangerous CSS');
                return false;
            }

            return true;
        };

        // Security helper functions
        const isValidImageUrl = (url) => {
            try {
                const urlObj = new URL(url, window.location.origin);
                const allowedProtocols = ['http:', 'https:'];
                const allowedDomains = [
                    window.location.hostname,
                    'exalt-account-manager.eu',
                    'api.exalt-account-manager.eu',
                    'exaltaccountmanager.com',
                    'api.exaltaccountmanager.com',
                    'news.api.exaltaccountmanager.com',
                ];

                if (!allowedProtocols.includes(urlObj.protocol)) return false;
                if (url.startsWith('/') || url.startsWith('./')) return true;
                return allowedDomains.some(domain => urlObj.hostname.endsWith(domain));
            } catch {
                return url.startsWith('/') || url.startsWith('./');
            }
        };

        const isValidActionUrl = (url) => {
            try {
                const urlObj = new URL(url);
                const trustedDomains = [
                    'github.com',
                    'exaltaccountmanager.com',
                    'api.exaltaccountmanager.com',
                    'exalt-account-manager.eu',
                    'api.exalt-account-manager.eu',
                ];

                if (urlObj.protocol !== 'https:') return false;
                return trustedDomains.some(domain => urlObj.hostname.endsWith(domain));
            } catch {
                return false;
            }
        };

        const containsDangerousContent = (content) => {
            const dangerousPatterns = [
                /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi,
                /javascript:/gi,
                /data:text\/html/gi,
                /vbscript:/gi,
                /[\s<]on(abort|blur|change|click|dblclick|error|focus|keydown|keypress|keyup|load|mousedown|mousemove|mouseout|mouseover|mouseup|reset|resize|select|submit|unload)\s*=/gi,
            ];

            return dangerousPatterns.some(pattern => pattern.test(content));
        };

        const containsDangerousStyles = (styles) => {
            if (typeof styles !== 'object' || !styles) return false;

            // More specific validation - check actual style property names, not substrings
            const dangerousStyleProps = [
                'content', // CSS content property (not justifyContent, etc.)
                'expression', // IE specific
            ];

            const styleString = JSON.stringify(styles).toLowerCase();
            const dangerousPatterns = [
                /javascript:/gi,
                /data:text\/html/gi,
                /expression\s*\(/gi,
                /url\s*\(\s*["']?\s*javascript:/gi,
            ];

            // Check for dangerous patterns in style values
            if (dangerousPatterns.some(pattern => pattern.test(styleString))) {
                return true;
            }

            // Check for exact dangerous property names (not substrings)
            const styleKeys = Object.keys(styles);
            return dangerousStyleProps.some(prop => styleKeys.includes(prop)) ||
                styleKeys.some(key => key === 'backgroundImage' &&
                    typeof styles[key] === 'string' &&
                    /url\s*\(/i.test(styles[key]));
        }; if (!validateProps()) {
            // Return null or a safe fallback for invalid elements
            return (
                <Box key={key} sx={{
                    p: 1,
                    border: '1px dashed',
                    borderColor: 'warning.main',
                    borderRadius: 1,
                    backgroundColor: 'warning.light',
                    opacity: 0.7
                }}>
                    <Typography variant="caption" color="warning.dark">
                        ⚠️ Invalid content element (type: {type})
                    </Typography>
                </Box>
            );
        }

        switch (type) {
            case 'news_card':
                return (
                    <NewsCard
                        key={key}
                        newsItem={content}
                    />
                );

            case 'component_box':
                return (
                    <ComponentBox
                        key={key}
                        {...props}
                        title={
                            props.title && typeof props.title === 'string' ? props.title : props.title?.map((title, index) => renderElement(title, index))
                        }
                    >
                        {children?.map((child, index) => renderElement(child, index))}
                    </ComponentBox>
                );

            case 'box':
                return (
                    <Box key={key} {...props}>
                        {children?.map((child, index) => renderElement(child, index))}
                    </Box>
                );

            case 'paper':
                return (
                    <Paper key={key} {...props}>
                        {children?.map((child, index) => renderElement(child, index))}
                    </Paper>
                );

            case 'text':
                return (
                    <Typography key={key} {...props}>
                        {
                            content ?
                                content
                                : children ?
                                    children.map((child, index) => renderElement(child, index))
                                    : null
                        }
                    </Typography>
                );

            case 'image':
                return (
                    <Box
                        key={key}
                        component="img"
                        src={src}
                        alt={alt}
                        {...props}
                    />
                );

            case 'icon':
                {
                    let Icon = null;
                    try {
                        Icon = Icons[content];
                    } catch (error) {
                        console.warn('Failed to load icon:', error);
                    }

                    if (!Icon) {
                        return null;
                    }

                    return (
                        <Icon key={key} {...props} />
                    );
                }

            case 'button':
                return (
                    <StyledButton
                        key={key}
                        {...props}
                        onClick={() => action && handleAction(action)}
                        {...(action.type === 'external_link' ? { href: action.url, target: '_blank', rel: 'noopener noreferrer' } : {})}
                    >
                        {content}
                    </StyledButton>
                );

            case 'chip':
                return (
                    <Chip
                        key={key}
                        label={content}
                        {...props}
                        icon={
                            props.icon ? renderElement(props.icon) : null
                        }
                    />
                );

            case 'divider':
                return <Divider key={key} sx={{ my: 2, ...props.sx }} />;

            default:
                console.warn('Unknown element type:', type);
                return null;
        }
    };

    return (
        <>
            {content?.map((element, index) => renderElement(element, index))}
        </>
    );
}

function NewsComponent({ news, onAction }) {

    // Validate the overall news structure
    const validateNewsStructure = (newsItem) => {
        if (!newsItem || typeof newsItem !== 'object') return false;

        // Required fields
        const requiredFields = ['id', 'title', 'timestamp', 'content'];
        if (!requiredFields.every(field => newsItem[field])) {
            console.warn('News item missing required fields:', requiredFields.filter(field => !newsItem[field]));
            return false;
        }

        // Validate ID format (should be safe string)
        if (!/^[a-zA-Z0-9\-_]+$/.test(newsItem.id)) {
            console.warn('Invalid news ID format:', newsItem.id);
            return false;
        }

        // Validate timestamp
        if (isNaN(Date.parse(newsItem.timestamp))) {
            console.warn('Invalid timestamp:', newsItem.timestamp);
            return false;
        }

        // Validate expiry date if present
        if (newsItem.expiryDate && isNaN(Date.parse(newsItem.expiryDate))) {
            console.warn('Invalid expiry date:', newsItem.expiryDate);
            return false;
        }

        // Validate priority and category enums
        const validPriorities = ['high', 'medium', 'low'];
        const validCategories = ['update', 'announcement', 'maintenance', 'feature'];

        if (newsItem.priority && !validPriorities.includes(newsItem.priority)) {
            console.warn('Invalid priority:', newsItem.priority);
            return false;
        }

        if (newsItem.category && !validCategories.includes(newsItem.category)) {
            console.warn('Invalid category:', newsItem.category);
            return false;
        }

        // Validate content is array
        if (!Array.isArray(newsItem.content)) {
            console.warn('News content must be an array');
            return false;
        }

        return true;
    };

    if (!news || !validateNewsStructure(news)) {
        return (
            <ComponentBox
                title="⚠️ Invalid News Content"
                sx={{ m: 1, borderColor: 'error.main' }}
            >
                <Typography variant="body2" color="error.main">
                    This news item could not be displayed due to invalid or potentially unsafe content.
                </Typography>
            </ComponentBox>
        );
    }

    return (
        <Box
            sx={{
                width: '100%',
                height: '100%',
            }}
        >
        <NewsRenderer
            content={news.content}
            onAction={onAction}
            />
            </Box>
    );
}

export default NewsComponent;
export { NewsRenderer };