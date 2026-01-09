import { Pause, Play } from "lucide-react";
import React, { useEffect, useRef, useState } from "react";

import Equaliser from "../Equaliser/Equaliser";
import styles from "./Booth.module.css";
import { useIsMobile } from "../../../hooks/useIsMobile";

interface BoothProps {
  title: string;
  artist: string;
  url: string;
  original: string;
  onEnded?: () => void;
  autoPlay?: boolean;
  onPlayPause?: (playing: boolean) => void;
}

const Booth: React.FC<BoothProps> = ({
  title,
  artist,
  url,
  original,
  onEnded,
  autoPlay = false,
  onPlayPause,
}) => {
  const [isPlaying, setIsPlaying] = useState(false);
  const audioRef = useRef<HTMLAudioElement>(null);

  useEffect(() => {
    if (autoPlay && audioRef.current) {
      audioRef.current.play().catch(() => {
        console.warn("waiting on user interaction");
      });
      setIsPlaying(true);
    } else {
      audioRef.current?.pause();
      setIsPlaying(false);
    }
  }, [url, autoPlay]);

  const togglePlay = () => {
    const audio = audioRef.current;
    if (!audio) return;

    if (isPlaying) {
      audio.pause();
      setIsPlaying(false);
      onPlayPause?.(false);
    } else {
      audio.play().catch(() => {});
      setIsPlaying(true);
      onPlayPause?.(true);
    }
  };

  const wrapperRef = useRef<HTMLDivElement>(null);
  const titleRef = useRef<HTMLAnchorElement>(null);
  const [isOverflowing, setIsOverflowing] = useState(false);

  useEffect(() => {
    const wrapper = wrapperRef.current;
    const text = titleRef.current;

    if (!wrapper || !text) return;

    setIsOverflowing(text.scrollWidth > wrapper.clientWidth);
  }, [title]);

  const isMobile = useIsMobile();

  return (
    <div
      className={`position-fixed bottom-0 start-0 m-3 d-flex align-items-center justify-content-between bg-dark text-white rounded-3 px-3 py-2${
        !isMobile ? " shadow" : ""
      }`}
      style={{ width: "310px", zIndex: 1050 }}
    >
      <div className="d-flex align-items-center gap-2">
        <Equaliser isPlaying={isPlaying} />
        <div
          ref={wrapperRef}
          style={{
            maxWidth: "150px",
            overflow: "hidden",
            whiteSpace: "nowrap",
          }}
        >
          <a
            ref={titleRef}
            href={original}
            target="_blank"
            rel="noopener noreferrer"
            className={`fw-medium small ${
              isOverflowing ? styles.scrollText : ""
            }`}
            style={{
              color: "inherit",
              textDecoration: "none",
              cursor: "pointer",
              display: "inline-block",
            }}
            onMouseEnter={(e) =>
              (e.currentTarget.style.textDecoration = "underline")
            }
            onMouseLeave={(e) =>
              (e.currentTarget.style.textDecoration = "none")
            }
          >
            {isOverflowing ? (
              <>
                <span style={{ paddingRight: "30px" }}>{title}</span>
                <span style={{ paddingRight: "30px" }}>{title}</span>
              </>
            ) : (
              <>{title}</>
            )}
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
          onClick={togglePlay}
        >
          {isPlaying ? <Pause size={22} /> : <Play size={22} />}
        </div>
      </div>

      <audio ref={audioRef} src={url} onEnded={onEnded} preload="auto" />
    </div>
  );
};

export default Booth;
