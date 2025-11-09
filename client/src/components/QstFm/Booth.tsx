import React, { useEffect, useRef, useState } from "react";
import { Play, Pause } from "lucide-react";
import Equaliser from "./Equaliser/Equaliser";

interface BoothProps {
  title: string;
  artist: string;
  url: string;
  isActive?: boolean;
  onEnded?: () => void;
}

const Booth: React.FC<BoothProps> = ({
  title,
  artist,
  url,
  isActive = true,
  onEnded,
}) => {
  const [isPlaying, setIsPlaying] = useState(false);
  const audioRef = useRef<HTMLAudioElement | null>(null);

  useEffect(() => {
    if (!audioRef.current) {
      audioRef.current = new Audio(url);
      audioRef.current.addEventListener("ended", () => {
        setIsPlaying(false);
        onEnded?.();
      });
    } else {
      audioRef.current.src = url;
    }

    if (isActive && isPlaying) {
      audioRef.current.play().catch(() => {});
    } else {
      audioRef.current.pause();
    }

    return () => {
      audioRef.current?.pause();
    };
  }, [url, isPlaying, isActive]);

  return (
    <div
      className="position-fixed bottom-0 start-0 m-3 d-flex align-items-center justify-content-between bg-dark text-white rounded-3 px-3 py-2 shadow"
      style={{ width: "300px", zIndex: 1050 }}
    >
      <div className="d-flex align-items-center gap-2">
        <Equaliser isPlaying={isPlaying} />
        <div>
          <a
            href={url}
            target="_blank"
            rel="noopener noreferrer"
            className="fw-medium small"
            style={{
              color: "inherit",
              textDecoration: "none",
              cursor: "pointer",
            }}
            onMouseEnter={(e) =>
              (e.currentTarget.style.textDecoration = "underline")
            }
            onMouseLeave={(e) =>
              (e.currentTarget.style.textDecoration = "none")
            }
          >
            {title}
          </a>
          <div
            className="text-secondary text-uppercase"
            style={{ fontSize: "0.7rem" }}
          >
            {artist}
          </div>
        </div>
      </div>

      <div
        className="position-relative"
        style={{
          height: "50px",
          borderRadius: "4px",
          overflow: "hidden",
        }}
      >
        <img
          src={
            "https://cdn.allenmaygibson.com/images/static/window-on-the-pointes.jpg"
          }
          alt="where imag"
          style={{
            width: "100%",
            height: "100%",
            objectFit: "cover",
            borderRadius: "4px",
            maskImage:
              "linear-gradient(to right, rgba(0,0,0,0.2) 0%, black 70%)",
            WebkitMaskImage:
              "linear-gradient(to right, rgba(0,0,0,0.2) 0%, black 70%)",
          }}
        />

        <div
          className="position-absolute top-50 end-0 translate-middle-y me-2"
          style={{ cursor: "pointer" }}
          onClick={() => setIsPlaying((p) => !p)}
        >
          {isPlaying ? <Pause size={22} /> : <Play size={22} />}
        </div>
      </div>
    </div>
  );
};

export default Booth;
