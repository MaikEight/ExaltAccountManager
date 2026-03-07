import useWidgets from "../../../../hooks/useWidgets";
import { Box } from '@mui/material';
import { Widgets } from "../../Widgets";
import { useEffect, useRef, useState } from 'react';
import {
    DndContext,
    closestCenter,
    KeyboardSensor,
    PointerSensor,
    useSensor,
    useSensors,
    DragOverlay,
} from '@dnd-kit/core';
import {
    arrayMove,
    SortableContext,
    sortableKeyboardCoordinates,
    verticalListSortingStrategy,
} from '@dnd-kit/sortable';

/**
 * This component renders all active widgets for the current WidgetBar Type in a grid layout.
 * The Grid has fixed dimensions per column and the amount of columns is determined by the slots in the WidgetBar settings.
 * Each widget can span multiple columns depending on its size settings.
 * Each widget can have its own height depending on its content.
 * Widgets can be rearranged via drag and drop.
 * @returns JSX Component
 */
function WidgetGrid() {
    const { widgetBarState, widgetBarConfig, reorderWidgets, updateWidgetBarEditMode } = useWidgets();
    const [activeId, setActiveId] = useState(null);

    const emptyRef = useRef(null);
    const textRef = useRef(null);
    const [arrowData, setArrowData] = useState(null);

    const slots = widgetBarConfig?.slots || 1;
    const editMode = widgetBarState?.editMode || false;
    const activeWidgets = widgetBarConfig?.activeWidgets || [];

    useEffect(() => {
        if (activeWidgets.length > 0) {
            setArrowData(null);
            return;
        }
        const el = emptyRef.current;
        if (!el) return;
        const update = () => {
            const { width, height } = el.getBoundingClientRect();
            if (!width || !height) return;
            const sx = width / 2;
            const textEl = textRef.current;
            const textBottom = textEl
                ? textEl.getBoundingClientRect().bottom - el.getBoundingClientRect().top + 8
                : height / 2 + 20;
            const sy = textBottom;
            const ex = width - 36;
            const ey = height - 18;
            // cubic bezier: go down first, then curve right toward the FAB
            const cp1x = sx;
            const cp1y = sy + (ey - sy) * 0.5;
            const cp2x = ex - (ex - sx) * 0.4;
            const cp2y = ey;
            setArrowData({
                path: `M ${sx} ${sy} C ${cp1x} ${cp1y}, ${cp2x} ${cp2y}, ${ex} ${ey}`,
                width,
                height,
            });
        };
        const ro = new ResizeObserver(update);
        ro.observe(el);
        update();
        return () => ro.disconnect();
    }, [activeWidgets?.length]);

    const sensors = useSensors(
        useSensor(PointerSensor, {
            activationConstraint: {
                distance: 8, // 8px movement required before drag starts
            },
        }),
        useSensor(KeyboardSensor, {
            coordinateGetter: sortableKeyboardCoordinates,
        })
    );

    const handleDragStart = (event) => {
        setActiveId(event.active.id);
    };

    const handleDragEnd = (event) => {
        if (!editMode) return;

        const { active, over } = event;

        if (active.id !== over?.id && over) {
            const oldIndex = activeWidgets.indexOf(active.id);
            const newIndex = activeWidgets.indexOf(over.id);

            const newOrder = arrayMove(activeWidgets, oldIndex, newIndex);
            reorderWidgets(newOrder);
        }

        setActiveId(null);
    };

    const handleDragCancel = () => {
        setActiveId(null);
    };

    return (
        <DndContext
            sensors={sensors}
            collisionDetection={closestCenter}
            onDragStart={handleDragStart}
            onDragEnd={handleDragEnd}
            onDragCancel={handleDragCancel}
        >
            <SortableContext
                items={activeWidgets}
                strategy={verticalListSortingStrategy}
                disabled={!editMode}
            >
                <Box
                    sx={{
                        display: 'grid',
                        gridTemplateColumns: `repeat(${slots}, minmax(0, 1fr))`,
                        gap: 1,
                        width: '100%',
                        ...(activeWidgets.length === 0 && { height: '100%' }),
                    }}
                >
                    {activeWidgets.length === 0 && (
                        <Box
                            ref={emptyRef}
                            sx={{
                                gridColumn: `span ${slots}`,
                                height: '100%',
                                position: 'relative',
                                display: 'flex',
                                alignItems: 'center',
                                justifyContent: 'center',
                                p: 2,
                                cursor: 'pointer',
                            }}
                        >
                            <Box
                                sx={{
                                    display: 'flex',
                                    flexDirection: 'column',
                                    alignItems: 'center',
                                    color: 'text.secondary',
                                }}
                            >
                                <img src="/mascot/Notification/reminder.png" alt="No widgets" width={192} height={96} style={{ marginBottom: 8 }} />
                                <span ref={textRef}>To add widgets, click here</span>
                            </Box>
                            {arrowData && (
                                <svg
                                    style={{
                                        position: 'absolute',
                                        top: 0,
                                        left: 0,
                                        width: arrowData.width,
                                        height: arrowData.height,
                                        pointerEvents: 'none',
                                        overflow: 'visible',
                                    }}
                                >
                                    <defs>
                                        <marker
                                            id="eam-wg-arrow"
                                            markerWidth="10"
                                            markerHeight="10"
                                            refX="9"
                                            refY="5"
                                            orient="auto"
                                            markerUnits="userSpaceOnUse"
                                        >
                                            <path d="M0,1 L9,5 L0,9 L2.5,5 Z" fill="currentColor" fillOpacity="0.35" />
                                        </marker>
                                    </defs>
                                    <path
                                        d={arrowData.path}
                                        fill="none"
                                        stroke="currentColor"
                                        strokeWidth="1.5"
                                        strokeOpacity="0.3"
                                        strokeDasharray="5 3"
                                        markerEnd="url(#eam-wg-arrow)"
                                    />
                                </svg>
                            )}
                        </Box>
                    )}
                    {
                        activeWidgets.map((widgettype) => {

                            const type = Widgets.getWidgetByType(widgettype);

                            if (!type) return null;
                            const WidgetComponent = type.Component;

                            return (
                                <WidgetComponent
                                    key={widgettype}
                                    type={type}
                                    widgetId={widgettype}
                                />
                            );
                        })
                    }
                </Box>
            </SortableContext>
            <DragOverlay>
                {activeId ? (() => {
                    const type = Widgets.getWidgetByType(activeId);
                    if (!type) return null;
                    const WidgetComponent = type.Component;

                    return (
                        <Box
                            sx={{
                                width: '100%',
                                opacity: 0.9,
                            }}
                        >
                            <WidgetComponent
                                type={type}
                                widgetId={activeId}
                            />
                        </Box>
                    );
                })() : null}
            </DragOverlay>
        </DndContext>
    );
}

export default WidgetGrid;