import { useEffect, useRef } from "react";

/**
 * Renders an animated image (APNG/GIF) at a reduced playback speed.
 *
 * Strategy: The source <img> is rendered visibly so the browser keeps animating
 * it. A <canvas> is absolutely positioned on top and samples the img at a lower
 * fps, effectively showing a slowed-down version to the user.
 *
 * @param {string}  src       - Image URL
 * @param {string}  alt       - Alt text
 * @param {number}  [fps=12]  - Target canvas sampling rate in frames per second
 * @param {object}  [style]   - Style applied to the outer wrapper div
 */
function SlowAnimatedImage({ src, alt, fps = 12, style }) {
    const canvasRef = useRef(null);
    const imgRef = useRef(null);

    useEffect(() => {
        const canvas = canvasRef.current;
        const img = imgRef.current;
        if (!canvas || !img) return;

        const ctx = canvas.getContext('2d');
        const interval = 1000 / fps;
        let lastDrawTime = 0;
        let rafId;

        const draw = (timestamp) => {
            rafId = requestAnimationFrame(draw);
            if (timestamp - lastDrawTime < interval) return;
            if (!img.complete || img.naturalWidth === 0) return;
            lastDrawTime = timestamp;
            canvas.width = img.naturalWidth;
            canvas.height = img.naturalHeight;
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.drawImage(img, 0, 0);
        };

        const start = () => { rafId = requestAnimationFrame(draw); };

        if (img.complete && img.naturalWidth > 0) {
            start();
        } else {
            img.onload = start;
        }

        return () => cancelAnimationFrame(rafId);
    }, [src, fps]);

    return (
        <div style={{ position: 'relative', display: 'inline-flex', ...style }}>
            {/* Kept fully visible so the browser continues animating the APNG */}
            <img ref={imgRef} src={src} alt={alt} style={{ display: 'block', width: '100%', height: '100%' }} />
            {/* Canvas overlays the img and repaints at the reduced fps */}
            <canvas ref={canvasRef} style={{ position: 'absolute', inset: 0, width: '100%', height: '100%' }} />
        </div>
    );
}

export default SlowAnimatedImage;


